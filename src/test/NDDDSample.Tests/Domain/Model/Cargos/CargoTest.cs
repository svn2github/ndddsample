namespace NDDDSample.Tests.Domain.Model.Cargos
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using Application.Utils;
    using NDDDSample.Domain.Model.Cargos;
    using NDDDSample.Domain.Model.Handlings;
    using NDDDSample.Domain.Model.Locations;
    using NDDDSample.Domain.Model.Voyages;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class CargoTest
    {
        private static readonly DateTime dateTime = new DateTime(2010, 6, 12);
        private List<HandlingEvent> events;
        private Voyage voyage;

        [SetUp]
        protected void setUp()
        {
            events = new List<HandlingEvent>();

            voyage = new Voyage.Builder(new VoyageNumber("0123"), SampleLocations.STOCKHOLM).
                AddMovement(SampleLocations.HAMBURG, dateTime, dateTime).
                AddMovement(SampleLocations.HONGKONG, dateTime, dateTime).
                AddMovement(SampleLocations.MELBOURNE, dateTime, dateTime).
                Build();
        }

        [Test]
        public void testConstruction()
        {
            var trackingId = new TrackingId("XYZ");
            var arrivalDeadline = DateTestUtil.toDate("2009-03-13");
            var routeSpecification = new RouteSpecification(
                SampleLocations.STOCKHOLM, SampleLocations.MELBOURNE, arrivalDeadline);

            Cargo cargo = new Cargo(trackingId, routeSpecification);

            Assert.AreEqual(RoutingStatus.NOT_ROUTED, cargo.Delivery.RoutingStatus);
            Assert.AreEqual(TransportStatus.NOT_RECEIVED, cargo.Delivery.TransportStatus);
            Assert.AreEqual(Location.UNKNOWN, cargo.Delivery.LastKnownLocation);
            Assert.AreEqual(Voyage.NONE, cargo.Delivery.CurrentVoyage);
        }

        [Test, Ignore("TODO: atrosin revise test how to port java specifics")]
        public void testRoutingStatus()
        {
            /*  //TODO: atrosin revise test how to port java specifics
            Cargo cargo = new Cargo(new TrackingId("XYZ"),
                                    new RouteSpecification(SampleLocations.STOCKHOLM, SampleLocations.MELBOURNE,
                                                           dateTime));
            Itinerary good = new Itinerary();
            Itinerary bad = new Itinerary();

            RouteSpecification acceptOnlyGood = new RouteSpecification(cargo.Origin,
                                                                       cargo.RouteSpecification.Destination, dateTime) 
                {
              @Override
              public boolean isSatisfiedBy(Itinerary itinerary) {
                return itinerary == good;
              }
            };

            cargo.SpecifyNewRoute(acceptOnlyGood);

            Assert.AreEqual(RoutingStatus.NOT_ROUTED, cargo.Delivery.RoutingStatus);

            cargo.AssignToRoute(bad);
            Assert.AreEqual(RoutingStatus.MISROUTED, cargo.Delivery.RoutingStatus);

            cargo.AssignToRoute(good);
            Assert.AreEqual(RoutingStatus.ROUTED, cargo.Delivery.RoutingStatus);*/
        }

        [Test]
        public void testlastKnownLocationUnknownWhenNoEvents()
        {
            Cargo cargo = new Cargo(new TrackingId("XYZ"),
                                    new RouteSpecification(SampleLocations.STOCKHOLM, SampleLocations.MELBOURNE,
                                                           dateTime));

            Assert.AreEqual(Location.UNKNOWN, cargo.Delivery.LastKnownLocation);
        }

        [Test]
        public void testlastKnownLocationReceived()
        {
            Cargo cargo = populateCargoReceivedStockholm();

            Assert.AreEqual(SampleLocations.STOCKHOLM, cargo.Delivery.LastKnownLocation);
        }

        [Test]
        public void testlastKnownLocationClaimed()
        {
            Cargo cargo = populateCargoClaimedMelbourne();

            Assert.AreEqual(SampleLocations.MELBOURNE, cargo.Delivery.LastKnownLocation);
        }

        [Test]
        public void testlastKnownLocationUnloaded()
        {
            Cargo cargo = populateCargoOffHongKong();

            Assert.AreEqual(SampleLocations.HONGKONG, cargo.Delivery.LastKnownLocation);
        }

        [Test]
        public void testlastKnownLocationloaded()
        {
            Cargo cargo = populateCargoOnHamburg();

            Assert.AreEqual(SampleLocations.HAMBURG, cargo.Delivery.LastKnownLocation);
        }

        [Test]
        public void testEquality()
        {
            RouteSpecification spec1 = new RouteSpecification(SampleLocations.STOCKHOLM, SampleLocations.HONGKONG,
                                                              dateTime);
            RouteSpecification spec2 = new RouteSpecification(SampleLocations.STOCKHOLM, SampleLocations.MELBOURNE,
                                                              dateTime);
            Cargo c1 = new Cargo(new TrackingId("ABC"), spec1);
            Cargo c2 = new Cargo(new TrackingId("CBA"), spec1);
            Cargo c3 = new Cargo(new TrackingId("ABC"), spec2);
            Cargo c4 = new Cargo(new TrackingId("ABC"), spec1);

            Assert.IsTrue(c1.Equals(c4), "Cargos should be equal when TrackingIDs are equal");
            Assert.IsTrue(c1.Equals(c3), "Cargos should be equal when TrackingIDs are equal");
            Assert.IsTrue(c3.Equals(c4), "Cargos should be equal when TrackingIDs are equal");
            Assert.IsFalse(c1.Equals(c2), "Cargos are not equal when TrackingID differ");
        }

        [Test]
        public void testIsUnloadedAtFinalDestination()
        {
            Cargo cargo = setUpCargoWithItinerary(SampleLocations.HANGZOU, SampleLocations.TOKYO,
                                                  SampleLocations.NEWYORK);
            Assert.IsFalse(cargo.Delivery.IsUnloadedAtDestination);

            // Adding an event unrelated to unloading at  destination
            events.Add(
                new HandlingEvent(cargo, dateTime.AddHours(10), dateTime, HandlingType.RECEIVE, SampleLocations.HANGZOU));
            cargo.DeriveDeliveryProgress(new HandlingHistory(events));
            Assert.IsFalse(cargo.Delivery.IsUnloadedAtDestination);

            Voyage voyage = new Voyage.Builder(new VoyageNumber("0123"), SampleLocations.HANGZOU).
                AddMovement(SampleLocations.NEWYORK, dateTime, dateTime).
                Build();

            // Adding an unload event, but not at the final destination
            events.Add(
                new HandlingEvent(cargo, dateTime.AddHours(20), dateTime, HandlingType.UNLOAD, SampleLocations.TOKYO,
                                  voyage));
            cargo.DeriveDeliveryProgress(new HandlingHistory(events));
            Assert.IsFalse(cargo.Delivery.IsUnloadedAtDestination);

            // Adding an event in the final destination, but not unload
            events.Add(
                new HandlingEvent(cargo, dateTime.AddHours(30), dateTime, HandlingType.CUSTOMS, SampleLocations.NEWYORK));
            cargo.DeriveDeliveryProgress(new HandlingHistory(events));
            Assert.IsFalse(cargo.Delivery.IsUnloadedAtDestination);

            // Finally, cargo is unloaded at final destination
            events.Add(
                new HandlingEvent(cargo, dateTime.AddHours(40), dateTime, HandlingType.UNLOAD, SampleLocations.NEWYORK,
                                  voyage));
            cargo.DeriveDeliveryProgress(new HandlingHistory(events));
            Assert.IsTrue(cargo.Delivery.IsUnloadedAtDestination);
        }

        [Test]
        // TODO: Generate test data some better way
        private Cargo populateCargoReceivedStockholm()
        {
            Cargo cargo = new Cargo(new TrackingId("XYZ"),
                                    new RouteSpecification(SampleLocations.STOCKHOLM, SampleLocations.MELBOURNE,
                                                           dateTime));

            HandlingEvent he = new HandlingEvent(cargo, getDate("2007-12-01"), dateTime, HandlingType.RECEIVE,
                                                 SampleLocations.STOCKHOLM);
            events.Add(he);
            cargo.DeriveDeliveryProgress(new HandlingHistory(events));

            return cargo;
        }

        [Test]
        private Cargo populateCargoClaimedMelbourne()
        {
            Cargo cargo = populateCargoOffMelbourne();

            events.Add(new HandlingEvent(cargo, getDate("2007-12-09"), dateTime, HandlingType.CLAIM,
                                         SampleLocations.MELBOURNE));
            cargo.DeriveDeliveryProgress(new HandlingHistory(events));

            return cargo;
        }

        [Test]
        private Cargo populateCargoOffHongKong()
        {
            Cargo cargo = new Cargo(new TrackingId("XYZ"),
                                    new RouteSpecification(SampleLocations.STOCKHOLM, SampleLocations.MELBOURNE,
                                                           dateTime));


            events.Add(new HandlingEvent(cargo, getDate("2007-12-01"), dateTime, HandlingType.LOAD,
                                         SampleLocations.STOCKHOLM, voyage));
            events.Add(new HandlingEvent(cargo, getDate("2007-12-02"), dateTime, HandlingType.UNLOAD,
                                         SampleLocations.HAMBURG, voyage));

            events.Add(new HandlingEvent(cargo, getDate("2007-12-03"), dateTime, HandlingType.LOAD,
                                         SampleLocations.HAMBURG, voyage));
            events.Add(new HandlingEvent(cargo, getDate("2007-12-04"), dateTime, HandlingType.UNLOAD,
                                         SampleLocations.HONGKONG, voyage));

            cargo.DeriveDeliveryProgress(new HandlingHistory(events));
            return cargo;
        }

        [Test]
        private Cargo populateCargoOnHamburg()
        {
            Cargo cargo = new Cargo(new TrackingId("XYZ"),
                                    new RouteSpecification(SampleLocations.STOCKHOLM, SampleLocations.MELBOURNE,
                                                           dateTime));

            events.Add(new HandlingEvent(cargo, getDate("2007-12-01"), dateTime, HandlingType.LOAD,
                                         SampleLocations.STOCKHOLM, voyage));
            events.Add(new HandlingEvent(cargo, getDate("2007-12-02"), dateTime, HandlingType.UNLOAD,
                                         SampleLocations.HAMBURG, voyage));
            events.Add(new HandlingEvent(cargo, getDate("2007-12-03"), dateTime, HandlingType.LOAD,
                                         SampleLocations.HAMBURG, voyage));

            cargo.DeriveDeliveryProgress(new HandlingHistory(events));
            return cargo;
        }

        [Test]
        private Cargo populateCargoOffMelbourne()
        {
            Cargo cargo = new Cargo(new TrackingId("XYZ"),
                                    new RouteSpecification(SampleLocations.STOCKHOLM, SampleLocations.MELBOURNE,
                                                           dateTime));

            events.Add(new HandlingEvent(cargo, getDate("2007-12-01"), dateTime, HandlingType.LOAD,
                                         SampleLocations.STOCKHOLM, voyage));
            events.Add(new HandlingEvent(cargo, getDate("2007-12-02"), dateTime, HandlingType.UNLOAD,
                                         SampleLocations.HAMBURG, voyage));

            events.Add(new HandlingEvent(cargo, getDate("2007-12-03"), dateTime, HandlingType.LOAD,
                                         SampleLocations.HAMBURG, voyage));
            events.Add(new HandlingEvent(cargo, getDate("2007-12-04"), dateTime, HandlingType.UNLOAD,
                                         SampleLocations.HONGKONG, voyage));

            events.Add(new HandlingEvent(cargo, getDate("2007-12-05"), dateTime, HandlingType.LOAD,
                                         SampleLocations.HONGKONG, voyage));
            events.Add(new HandlingEvent(cargo, getDate("2007-12-07"), dateTime, HandlingType.UNLOAD,
                                         SampleLocations.MELBOURNE, voyage));

            cargo.DeriveDeliveryProgress(new HandlingHistory(events));
            return cargo;
        }
               
        [Test]
        public void testIsMisdirected()
        {
            //A cargo with no itinerary is not misdirected
            Cargo cargo = new Cargo(new TrackingId("TRKID"),
                                    new RouteSpecification(SampleLocations.SHANGHAI, SampleLocations.GOTHENBURG,
                                                           dateTime));
            Assert.IsFalse(cargo.Delivery.IsMisdirected);

            cargo = setUpCargoWithItinerary(SampleLocations.SHANGHAI, SampleLocations.ROTTERDAM,
                                            SampleLocations.GOTHENBURG);

            //A cargo with no handling events is not misdirected
            Assert.IsFalse(cargo.Delivery.IsMisdirected);

            var handlingEvents = new Collection<HandlingEvent>();

            //Happy path
            handlingEvents.Add(new HandlingEvent(cargo, dateTime.AddHours(10), dateTime.AddHours(20),
                                                 HandlingType.RECEIVE, SampleLocations.SHANGHAI));
            handlingEvents.Add(new HandlingEvent(cargo, dateTime.AddHours(30), dateTime.AddHours(40), HandlingType.LOAD,
                                                 SampleLocations.SHANGHAI, voyage));
            handlingEvents.Add(new HandlingEvent(cargo, dateTime.AddHours(50), dateTime.AddHours(60),
                                                 HandlingType.UNLOAD, SampleLocations.ROTTERDAM, voyage));
            handlingEvents.Add(new HandlingEvent(cargo, dateTime.AddHours(70), dateTime.AddHours(80), HandlingType.LOAD,
                                                 SampleLocations.ROTTERDAM, voyage));
            handlingEvents.Add(new HandlingEvent(cargo, dateTime.AddHours(90), dateTime.AddHours(100),
                                                 HandlingType.UNLOAD, SampleLocations.GOTHENBURG, voyage));
            handlingEvents.Add(new HandlingEvent(cargo, dateTime.AddHours(110), dateTime.AddHours(120),
                                                 HandlingType.CLAIM, SampleLocations.GOTHENBURG));
            handlingEvents.Add(new HandlingEvent(cargo, dateTime.AddHours(130), dateTime.AddHours(140),
                                                 HandlingType.CUSTOMS, SampleLocations.GOTHENBURG));

            events.AddRange(handlingEvents);
            cargo.DeriveDeliveryProgress(new HandlingHistory(events));
            Assert.IsFalse(cargo.Delivery.IsMisdirected);

            //Try a couple of failing ones

            cargo = setUpCargoWithItinerary(SampleLocations.SHANGHAI, SampleLocations.ROTTERDAM,
                                            SampleLocations.GOTHENBURG);

            handlingEvents = new Collection<HandlingEvent>
                                 {
                                     new HandlingEvent(cargo, dateTime, dateTime, HandlingType.RECEIVE,
                                                       SampleLocations.HANGZOU)
                                 };

            events.AddRange(handlingEvents);
            cargo.DeriveDeliveryProgress(new HandlingHistory(events));

            Assert.IsTrue(cargo.Delivery.IsMisdirected);


            cargo = setUpCargoWithItinerary(SampleLocations.SHANGHAI, SampleLocations.ROTTERDAM,
                                            SampleLocations.GOTHENBURG);

            handlingEvents = new Collection<HandlingEvent>
                                 {
                                     new HandlingEvent(cargo, dateTime.AddHours(10), dateTime.AddHours(20),
                                                       HandlingType.RECEIVE,
                                                       SampleLocations.SHANGHAI),
                                     new HandlingEvent(cargo, dateTime.AddHours(30), dateTime.AddHours(40),
                                                       HandlingType.LOAD,
                                                       SampleLocations.SHANGHAI, voyage),
                                     new HandlingEvent(cargo, dateTime.AddHours(50), dateTime.AddHours(60),
                                                       HandlingType.UNLOAD,
                                                       SampleLocations.ROTTERDAM, voyage),
                                     new HandlingEvent(cargo, dateTime.AddHours(70), dateTime.AddHours(80),
                                                       HandlingType.LOAD,
                                                       SampleLocations.ROTTERDAM, voyage)
                                 };

            events.AddRange(handlingEvents);
            cargo.DeriveDeliveryProgress(new HandlingHistory(events));

            Assert.IsTrue(cargo.Delivery.IsMisdirected);


            cargo = setUpCargoWithItinerary(SampleLocations.SHANGHAI, SampleLocations.ROTTERDAM,
                                            SampleLocations.GOTHENBURG);
            handlingEvents = new Collection<HandlingEvent>
                                 {
                                     new HandlingEvent(cargo, dateTime.AddHours(10), dateTime.AddHours(20),
                                                       HandlingType.RECEIVE,
                                                       SampleLocations.SHANGHAI),
                                     new HandlingEvent(cargo, dateTime.AddHours(30), dateTime.AddHours(40),
                                                       HandlingType.LOAD,
                                                       SampleLocations.SHANGHAI, voyage),
                                     new HandlingEvent(cargo, dateTime.AddHours(50), dateTime.AddHours(60),
                                                       HandlingType.UNLOAD,
                                                       SampleLocations.ROTTERDAM, voyage),
                                     new HandlingEvent(cargo, dateTime, dateTime, HandlingType.CLAIM,
                                                       SampleLocations.ROTTERDAM)
                                 };

            events.AddRange(handlingEvents);
            cargo.DeriveDeliveryProgress(new HandlingHistory(events));

            Assert.IsTrue(cargo.Delivery.IsMisdirected);
        }

        private Cargo setUpCargoWithItinerary(Location origin, Location midpoint, Location destination)
        {
            var cargo = new Cargo(new TrackingId("CARGO1"), new RouteSpecification(origin, destination, dateTime));

            var itinerary = new Itinerary(
                new List<Leg>
                    {
                        new Leg(voyage, origin, midpoint, dateTime, dateTime),
                        new Leg(voyage, midpoint, destination, dateTime, dateTime)
                    });

            cargo.AssignToRoute(itinerary);
            return cargo;
        }


        /// <summary>
        /// Parse an ISO 8601 (YYYY-MM-DD) String to Date    
        /// </summary>
        /// <param name="isoFormat">String to parse.</param>
        /// <returns>Created date instance.</returns>
        private static DateTime getDate(String isoFormat)
        {
            return DateTime.ParseExact(isoFormat, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
    }
}