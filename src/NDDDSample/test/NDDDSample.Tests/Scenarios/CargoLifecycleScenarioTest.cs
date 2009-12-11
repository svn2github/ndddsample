namespace NDDDSample.Tests.Scenarios
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using Application;
    using Application.Impl;   
    using Infrastructure.Messaging.Stub;
    using Infrastructure.Persistence.Inmemory;
    using NDDDSample.Domain.Model.Cargos;
    using NDDDSample.Domain.Model.Handlings;
    using NDDDSample.Domain.Model.Handlings.Exceptions;
    using NDDDSample.Domain.Model.Locations;
    using NDDDSample.Domain.Model.Voyages;
    using NDDDSample.Domain.Service;
    using NDDDSample.Infrastructure;
    using NUnit.Framework;

    #endregion

    [TestFixture, Category(UnitTestCategories.Scenarios)]
    public class CargoLifecycleScenarioTest
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            routingService = new RoutingServiceImpl();

            applicationEvents = new SynchronousApplicationEventsStub();

            // In-memory implementations of the repositories
            handlingEventRepository = new HandlingEventRepositoryInMem();
            cargoRepository = new CargoRepositoryInMem();
            locationRepository = new LocationRepositoryInMem();
            voyageRepository = new VoyageRepositoryInMem();

            // Actual factories and application services, wired with stubbed or in-memory infrastructure
            handlingEventFactory = new HandlingEventFactory(cargoRepository, voyageRepository, locationRepository);

            cargoInspectionService = new CargoInspectionService(applicationEvents, cargoRepository,
                                                                handlingEventRepository);
            handlingEventService = new HandlingEventService(handlingEventRepository, applicationEvents,
                                                            handlingEventFactory);
            bookingService = new BookingService(cargoRepository, locationRepository, routingService);

            // Circular dependency when doing synchrounous calls
            ((SynchronousApplicationEventsStub) applicationEvents).SetCargoInspectionService(cargoInspectionService);
        }

        #endregion

        /// <summary>
        /// Repository implementations are part of the infrastructure layer,
        /// which in this test is stubbed out by in-memory replacements.
        /// </summary>
        private IHandlingEventRepository handlingEventRepository;

        private ICargoRepository cargoRepository;
        private ILocationRepository locationRepository;
        private IVoyageRepository voyageRepository;

        /// <summary>
        /// This interface is part of the application layer,
        ///and defines a number of events that occur during
        ///aplication execution. It is used for message-driving
        ///and is implemented using JMS.
        ///In this test it is stubbed with synchronous calls.         
        /// </summary>
        private IApplicationEvents applicationEvents;

        /// <summary>      
        /// These three components all belong to the application layer,
        ///and map against use cases of the application. The "real"
        ///implementations are used in this lifecycle test,
        ///but wired with stubbed infrastructure.
        /// </summary>
        private IBookingService bookingService;

        private IHandlingEventService handlingEventService;
        private ICargoInspectionService cargoInspectionService;

        /// <summary>       
        /// This factory is part of the handling aggregate and belongs to
        ///the domain layer. Similar to the application layer components,
        ///the "real" implementation is used here too,
        ///wired with stubbed infrastructure.
        /// </summary>
        private HandlingEventFactory handlingEventFactory;

        /// <summary>
        /// This is a domain service interface, whose implementation
        ///is part of the infrastructure layer (remote call to external system).
        ///It is stubbed in this test.
        /// </summary>
        private IRoutingService routingService;

        #region Utility stubs

        private static Itinerary SelectPreferedItinerary(IList<Itinerary> itineraries)
        {
            return itineraries[0];
        }

        public class RoutingServiceImpl : IRoutingService
        {
            #region IRoutingService Members

            public IList<Itinerary> FetchRoutesForSpecification(RouteSpecification routeSpecification)
            {
                if (routeSpecification.Origin.Equals(SampleLocations.HONGKONG))
                {
                    var nsLegs = new List<Leg>
                                     {
                                         new Leg(SampleVoyages.v100, SampleLocations.HONGKONG, SampleLocations.NEWYORK,
                                                 DateUtil.ToDate("2009-03-03"), DateUtil.ToDate("2009-03-09")),
                                         new Leg(SampleVoyages.v200, SampleLocations.NEWYORK, SampleLocations.CHICAGO,
                                                 DateUtil.ToDate("2009-03-10"), DateUtil.ToDate("2009-03-14")),
                                         new Leg(SampleVoyages.v200, SampleLocations.CHICAGO, SampleLocations.STOCKHOLM,
                                                 DateUtil.ToDate("2009-03-07"), DateUtil.ToDate("2009-03-11"))
                                     };
                    // Hongkong - NYC - Chicago - Stockholm, initial routing
                    return new List<Itinerary> {new Itinerary(nsLegs)};
                }
                else
                {
                    var tsLegs = new List<Leg>
                                     {
                                         new Leg(SampleVoyages.v300, SampleLocations.TOKYO, SampleLocations.HAMBURG,
                                                 DateUtil.ToDate("2009-03-08"), DateUtil.ToDate("2009-03-12")),
                                         new Leg(SampleVoyages.v400, SampleLocations.HAMBURG, SampleLocations.STOCKHOLM,
                                                 DateUtil.ToDate("2009-03-14"), DateUtil.ToDate("2009-03-15"))
                                     };
                    // Tokyo - Hamburg - Stockholm, rerouting misdirected cargo from Tokyo 
                    return new List<Itinerary> {new Itinerary(tsLegs)};
                }
            }

            #endregion

            #endregion
        }

        [Test]
        public void testCargoFromHongkongToStockholm()
        {
            //Test setup: A cargo should be shipped from Hongkong to Stockholm,
            //and it should arrive in no more than two weeks. 
            Location origin = SampleLocations.HONGKONG;
            Location destination = SampleLocations.STOCKHOLM;
            DateTime arrivalDeadline = DateUtil.ToDate("2009-03-18");


            //  Use case 1: booking 
            //A new cargo is booked, and the unique tracking id is assigned to the cargo. 
            TrackingId trackingId = bookingService.BookNewCargo(
                origin.UnLocode, destination.UnLocode, arrivalDeadline);

            //      The tracking id can be used to lookup the cargo in the repository.
            //Important: The cargo, and thus the domain model, is responsible for determining
            //the status of the cargo, whether it is on the right track or not and so on.
            //This is core domain logic.

            //Tracking the cargo basically amounts to presenting information extracted from
            //the cargo aggregate in a suitable way. 
            Cargo cargo = cargoRepository.Find(trackingId);
            Assert.IsNotNull(cargo);
            Assert.AreEqual(TransportStatus.NOT_RECEIVED, cargo.Delivery.TransportStatus);
            Assert.AreEqual(RoutingStatus.NOT_ROUTED, cargo.Delivery.RoutingStatus);
            Assert.IsFalse(cargo.Delivery.IsMisdirected);
            Assert.AreEqual(cargo.Delivery.EstimatedTimeOfArrival, Delivery.ETA_UNKOWN);
            Assert.IsNull(cargo.Delivery.NextExpectedActivity);


            //   Use case 2: routing 
            //A number of possible routes for this cargo is requested and may be
            //presented to the customer in some way for him/her to choose from.
            //Selection could be affected by things like price and time of delivery,
            //but this test simply uses an arbitrary selection to mimic that process.

            //The cargo is then assigned to the selected route, described by an itinerary.
            IList<Itinerary> itineraries = bookingService.RequestPossibleRoutesForCargo(trackingId);
            Itinerary itinerary = SelectPreferedItinerary(itineraries);
            cargo.AssignToRoute(itinerary);

            Assert.AreEqual(TransportStatus.NOT_RECEIVED, cargo.Delivery.TransportStatus);
            Assert.AreEqual(RoutingStatus.ROUTED, cargo.Delivery.RoutingStatus);
            Assert.AreNotEqual(Delivery.ETA_UNKOWN, cargo.Delivery.EstimatedTimeOfArrival);
            Assert.AreEqual(new HandlingActivity(HandlingType.RECEIVE, SampleLocations.HONGKONG),
                            cargo.Delivery.NextExpectedActivity);


            //   Use case 3: handling

            //A handling event registration attempt will be formed from parsing
            //the data coming in as a handling report either via
            //the web service interface or as an uploaded CSV file.

            //The handling event factory tries to create a HandlingEvent from the attempt,
            //and if the factory decides that this is a plausible handling event, it is stored.
            //If the attempt is invalid, for example if no cargo exists for the specfied tracking id,
            //the attempt is rejected.

            //Handling begins: cargo is received in Hongkong.     
            handlingEventService.RegisterHandlingEvent(
                DateUtil.ToDate("2009-03-01"), trackingId, null, SampleLocations.HONGKONG.UnLocode,
                HandlingType.RECEIVE);

            Assert.AreEqual(TransportStatus.IN_PORT, cargo.Delivery.TransportStatus);
            Assert.AreEqual(SampleLocations.HONGKONG, cargo.Delivery.LastKnownLocation);

            // Next event: Load onto voyage CM003 in Hongkong
            handlingEventService.RegisterHandlingEvent(
                DateUtil.ToDate("2009-03-03"), trackingId, SampleVoyages.v100.VoyageNumber,
                SampleLocations.HONGKONG.UnLocode, HandlingType.LOAD);

            // Check current state - should be ok
            Assert.AreEqual(SampleVoyages.v100, cargo.Delivery.CurrentVoyage);
            Assert.AreEqual(SampleLocations.HONGKONG, cargo.Delivery.LastKnownLocation);
            Assert.AreEqual(TransportStatus.ONBOARD_CARRIER, cargo.Delivery.TransportStatus);
            Assert.IsFalse(cargo.Delivery.IsMisdirected);
            Assert.AreEqual(
                new HandlingActivity(HandlingType.UNLOAD, SampleLocations.NEWYORK, SampleVoyages.v100),
                cargo.Delivery.NextExpectedActivity);

            //Here's an attempt to register a handling event that's not valid
            //because there is no voyage with the specified voyage number,
            //and there's no location with the specified UN Locode either.

            //This attempt will be rejected and will not affect the cargo delivery in any way.
            VoyageNumber noSuchVoyageNumber = new VoyageNumber("XX000");
            UnLocode noSuchUnLocode = new UnLocode("ZZZZZ");
            try
            {
                handlingEventService.RegisterHandlingEvent(
                    DateUtil.ToDate("2009-03-05"), trackingId, noSuchVoyageNumber, noSuchUnLocode,
                    HandlingType.LOAD);
                Assert.Fail("Should not be able to register a handling event with invalid location and voyage");
            }
            catch (CannotCreateHandlingEventException expected) {}


            // Cargo is now (incorrectly) unloaded in Tokyo
            handlingEventService.RegisterHandlingEvent(
                DateUtil.ToDate("2009-03-05"), trackingId, SampleVoyages.v100.VoyageNumber,
                SampleLocations.TOKYO.UnLocode, HandlingType.UNLOAD);

            // Check current state - cargo is misdirected!
            Assert.AreEqual(Voyage.NONE, cargo.Delivery.CurrentVoyage);
            Assert.AreEqual(SampleLocations.TOKYO, cargo.Delivery.LastKnownLocation);
            Assert.AreEqual(TransportStatus.IN_PORT, cargo.Delivery.TransportStatus);
            Assert.IsTrue(cargo.Delivery.IsMisdirected);
            Assert.IsNull(cargo.Delivery.NextExpectedActivity);


            // -- Cargo needs to be rerouted --
            // TODO cleaner reroute from "earliest location from where the new route originates"

            // Specify a new route, this time from Tokyo (where it was incorrectly unloaded) to Stockholm
            RouteSpecification fromTokyo = new RouteSpecification(SampleLocations.TOKYO, SampleLocations.STOCKHOLM,
                                                                  arrivalDeadline);
            cargo.SpecifyNewRoute(fromTokyo);

            // The old itinerary does not satisfy the new specification
            Assert.AreEqual(RoutingStatus.MISROUTED, cargo.Delivery.RoutingStatus);
            Assert.IsNull(cargo.Delivery.NextExpectedActivity);

            // Repeat procedure of selecting one out of a number of possible routes satisfying the route spec
            IList<Itinerary> newItineraries = bookingService.RequestPossibleRoutesForCargo(cargo.TrackingId);
            Itinerary newItinerary = SelectPreferedItinerary(newItineraries);
            cargo.AssignToRoute(newItinerary);

            // New itinerary should satisfy new route
            Assert.AreEqual(RoutingStatus.ROUTED, cargo.Delivery.RoutingStatus);

            // TODO we can't handle the face that after a reroute, the cargo isn't misdirected anymore
            //Assert.IsFalse(cargo.isMisdirected());
            //Assert.AreEqual(new HandlingActivity(LOAD, TOKYO), cargo.nextExpectedActivity());


            // -- Cargo has been rerouted, shipping continues --


            // Load in Tokyo
            handlingEventService.RegisterHandlingEvent(
                DateUtil.ToDate("2009-03-08"), trackingId, SampleVoyages.v300.VoyageNumber,
                SampleLocations.TOKYO.UnLocode, HandlingType.LOAD);

            // Check current state - should be ok
            Assert.AreEqual(SampleVoyages.v300, cargo.Delivery.CurrentVoyage);
            Assert.AreEqual(SampleLocations.TOKYO, cargo.Delivery.LastKnownLocation);
            Assert.AreEqual(TransportStatus.ONBOARD_CARRIER, cargo.Delivery.TransportStatus);
            Assert.IsFalse(cargo.Delivery.IsMisdirected);
            Assert.AreEqual(
                new HandlingActivity(HandlingType.UNLOAD, SampleLocations.HAMBURG, SampleVoyages.v300),
                cargo.Delivery.NextExpectedActivity);

            // Unload in Hamburg
            handlingEventService.RegisterHandlingEvent(
                DateUtil.ToDate("2009-03-12"), trackingId, SampleVoyages.v300.VoyageNumber,
                SampleLocations.HAMBURG.UnLocode, HandlingType.UNLOAD);

            // Check current state - should be ok
            Assert.AreEqual(Voyage.NONE, cargo.Delivery.CurrentVoyage);
            Assert.AreEqual(SampleLocations.HAMBURG, cargo.Delivery.LastKnownLocation);
            Assert.AreEqual(TransportStatus.IN_PORT, cargo.Delivery.TransportStatus);
            Assert.IsFalse(cargo.Delivery.IsMisdirected);
            Assert.AreEqual(
                new HandlingActivity(HandlingType.LOAD, SampleLocations.HAMBURG, SampleVoyages.v400),
                cargo.Delivery.NextExpectedActivity);


            // Load in Hamburg
            handlingEventService.RegisterHandlingEvent(
                DateUtil.ToDate("2009-03-14"), trackingId, SampleVoyages.v400.VoyageNumber,
                SampleLocations.HAMBURG.UnLocode, HandlingType.LOAD);

            // Check current state - should be ok
            Assert.AreEqual(SampleVoyages.v400, cargo.Delivery.CurrentVoyage);
            Assert.AreEqual(SampleLocations.HAMBURG, cargo.Delivery.LastKnownLocation);
            Assert.AreEqual(TransportStatus.ONBOARD_CARRIER, cargo.Delivery.TransportStatus);
            Assert.IsFalse(cargo.Delivery.IsMisdirected);
            Assert.AreEqual(
                new HandlingActivity(HandlingType.UNLOAD, SampleLocations.STOCKHOLM, SampleVoyages.v400),
                cargo.Delivery.NextExpectedActivity);


            // Unload in Stockholm
            handlingEventService.RegisterHandlingEvent(
                DateUtil.ToDate("2009-03-15"), trackingId, SampleVoyages.v400.VoyageNumber,
                SampleLocations.STOCKHOLM.UnLocode, HandlingType.UNLOAD);

            // Check current state - should be ok
            Assert.AreEqual(Voyage.NONE, cargo.Delivery.CurrentVoyage);
            Assert.AreEqual(SampleLocations.STOCKHOLM, cargo.Delivery.LastKnownLocation);
            Assert.AreEqual(TransportStatus.IN_PORT, cargo.Delivery.TransportStatus);
            Assert.IsFalse(cargo.Delivery.IsMisdirected);
            Assert.AreEqual(new HandlingActivity(HandlingType.CLAIM, SampleLocations.STOCKHOLM),
                            cargo.Delivery.NextExpectedActivity);

            // Finally, cargo is claimed in Stockholm. This ends the cargo lifecycle from our perspective.
            handlingEventService.RegisterHandlingEvent(
                DateUtil.ToDate("2009-03-16"), trackingId, null, SampleLocations.STOCKHOLM.UnLocode,
                HandlingType.CLAIM);

            // Check current state - should be ok
            Assert.AreEqual(Voyage.NONE, cargo.Delivery.CurrentVoyage);
            Assert.AreEqual(SampleLocations.STOCKHOLM, cargo.Delivery.LastKnownLocation);
            Assert.AreEqual(TransportStatus.CLAIMED, cargo.Delivery.TransportStatus);
            Assert.IsFalse(cargo.Delivery.IsMisdirected);
            Assert.IsNull(cargo.Delivery.NextExpectedActivity);
        }
    }
}