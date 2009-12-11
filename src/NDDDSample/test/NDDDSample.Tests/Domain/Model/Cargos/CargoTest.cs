namespace NDDDSample.Tests.Domain.Model.Cargos
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;    
    using NDDDSample.Domain.Model.Cargos;
    using NDDDSample.Domain.Model.Handlings;
    using NDDDSample.Domain.Model.Locations;
    using NDDDSample.Domain.Model.Voyages;
    using NDDDSample.Infrastructure;
    using NUnit.Framework;

    #endregion

    [TestFixture, Category(UnitTestCategories.DomainModel)]
    public class CargoTest
    {      
        private List<HandlingEvent> events;
        private Voyage voyage;

        [SetUp]
        protected void SetUp()
        {
            events = new List<HandlingEvent>();

            voyage = new Voyage.Builder(new VoyageNumber("0123"), SampleLocations.STOCKHOLM).
                AddMovement(SampleLocations.HAMBURG, DateTime.Now, DateTime.Now).
                AddMovement(SampleLocations.HONGKONG, DateTime.Now, DateTime.Now).
                AddMovement(SampleLocations.MELBOURNE, DateTime.Now, DateTime.Now).
                Build();
        }

        [Test]
        public void TestConstruction()
        {
            var trackingId = new TrackingId("XYZ");
            var arrivalDeadline = DateUtil.ToDate("2009-03-13");
            var routeSpecification = new RouteSpecification(
                SampleLocations.STOCKHOLM, SampleLocations.MELBOURNE, arrivalDeadline);

            Cargo cargo = new Cargo(trackingId, routeSpecification);

            Assert.AreEqual(RoutingStatus.NOT_ROUTED, cargo.Delivery.RoutingStatus);
            Assert.AreEqual(TransportStatus.NOT_RECEIVED, cargo.Delivery.TransportStatus);
            Assert.AreEqual(Location.UNKNOWN, cargo.Delivery.LastKnownLocation);
            Assert.AreEqual(Voyage.NONE, cargo.Delivery.CurrentVoyage);
        }

        [Test]
        public void TestRoutingStatus()
        {       
            Cargo cargo = new Cargo(new TrackingId("XYZ"),
                                    new RouteSpecification(SampleLocations.STOCKHOLM, SampleLocations.MELBOURNE,
                                                           DateTime.Now));
            var good = new Itinerary();
            var bad = new Itinerary();

            RouteSpecification acceptOnlyGood = new RouteSpecificationStub(cargo.Origin,
                                                                           cargo.RouteSpecification.Destination,
                                                                           DateTime.Now, good);             
            cargo.SpecifyNewRoute(acceptOnlyGood);

            Assert.AreEqual(RoutingStatus.NOT_ROUTED, cargo.Delivery.RoutingStatus);

            cargo.AssignToRoute(bad);
            Assert.AreEqual(RoutingStatus.MISROUTED, cargo.Delivery.RoutingStatus);

            cargo.AssignToRoute(good);
            Assert.AreEqual(RoutingStatus.ROUTED, cargo.Delivery.RoutingStatus);
        }

        #region Nested RouteSpecificationStub Class

        //TODO: atrosin revise the class, mock it using mocking framework?
        public class RouteSpecificationStub: RouteSpecification
        {
            private readonly Itinerary good;

            public RouteSpecificationStub(Location origin, Location destination, DateTime arrivalDeadline, Itinerary good) 
                : base(origin, destination, arrivalDeadline)
            {
                this.good = good;
            }

            public override bool IsSatisfiedBy(Itinerary itinerary)
            {
                return itinerary == good;
            } 
        }

        #endregion

        [Test]
        public void TestlastKnownLocationUnknownWhenNoEvents()
        {
            Cargo cargo = new Cargo(new TrackingId("XYZ"),
                                    new RouteSpecification(SampleLocations.STOCKHOLM, SampleLocations.MELBOURNE,
                                                           DateTime.Now));

            Assert.AreEqual(Location.UNKNOWN, cargo.Delivery.LastKnownLocation);
        }

        [Test]
        public void TestlastKnownLocationReceived()
        {
            Cargo cargo = PopulateCargoReceivedStockholm();

            Assert.AreEqual(SampleLocations.STOCKHOLM, cargo.Delivery.LastKnownLocation);
        }

        [Test]
        public void TestlastKnownLocationClaimed()
        {
            Cargo cargo = PopulateCargoClaimedMelbourne();

            Assert.AreEqual(SampleLocations.MELBOURNE, cargo.Delivery.LastKnownLocation);
        }

        [Test]
        public void TestlastKnownLocationUnloaded()
        {
            Cargo cargo = PopulateCargoOffHongKong();

            Assert.AreEqual(SampleLocations.HONGKONG, cargo.Delivery.LastKnownLocation);
        }

        [Test]
        public void TestlastKnownLocationloaded()
        {
            Cargo cargo = PopulateCargoOnHamburg();

            Assert.AreEqual(SampleLocations.HAMBURG, cargo.Delivery.LastKnownLocation);
        }

        [Test]
        public void TestEquality()
        {
            RouteSpecification spec1 = new RouteSpecification(SampleLocations.STOCKHOLM, SampleLocations.HONGKONG,
                                                              DateTime.Now);
            RouteSpecification spec2 = new RouteSpecification(SampleLocations.STOCKHOLM, SampleLocations.MELBOURNE,
                                                              DateTime.Now);
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
        public void TestIsUnloadedAtFinalDestination()
        {
            Cargo cargo = SetUpCargoWithItinerary(SampleLocations.HANGZOU, SampleLocations.TOKYO,
                                                  SampleLocations.NEWYORK);
            Assert.IsFalse(cargo.Delivery.IsUnloadedAtDestination);

            // Adding an event unrelated to unloading at  destination
            events.Add(
                new HandlingEvent(cargo, new DateTime(10), DateTime.Now, HandlingType.RECEIVE, SampleLocations.HANGZOU));
            cargo.DeriveDeliveryProgress(new HandlingHistory(events));
            Assert.IsFalse(cargo.Delivery.IsUnloadedAtDestination);

            Voyage voyage = new Voyage.Builder(new VoyageNumber("0123"), SampleLocations.HANGZOU).
                AddMovement(SampleLocations.NEWYORK, DateTime.Now, DateTime.Now).
                Build();

            // Adding an unload event, but not at the final destination
            events.Add(
                new HandlingEvent(cargo, new DateTime(20), DateTime.Now, HandlingType.UNLOAD, SampleLocations.TOKYO,
                                  voyage));
            cargo.DeriveDeliveryProgress(new HandlingHistory(events));
            Assert.IsFalse(cargo.Delivery.IsUnloadedAtDestination);

            // Adding an event in the final destination, but not unload
            events.Add(
                new HandlingEvent(cargo, new DateTime(30), DateTime.Now, HandlingType.CUSTOMS, SampleLocations.NEWYORK));
            cargo.DeriveDeliveryProgress(new HandlingHistory(events));
            Assert.IsFalse(cargo.Delivery.IsUnloadedAtDestination);

            // Finally, cargo is unloaded at final destination
            events.Add(
                new HandlingEvent(cargo, new DateTime(40), DateTime.Now, HandlingType.UNLOAD, SampleLocations.NEWYORK,
                                  voyage));
            cargo.DeriveDeliveryProgress(new HandlingHistory(events));
            Assert.IsTrue(cargo.Delivery.IsUnloadedAtDestination);
        }

        [Test]
        // TODO: Generate test data some better way
        private Cargo PopulateCargoReceivedStockholm()
        {
            Cargo cargo = new Cargo(new TrackingId("XYZ"),
                                    new RouteSpecification(SampleLocations.STOCKHOLM, SampleLocations.MELBOURNE,
                                                           DateTime.Now));

            HandlingEvent he = new HandlingEvent(cargo, GetDate("2007-12-01"), DateTime.Now, HandlingType.RECEIVE,
                                                 SampleLocations.STOCKHOLM);
            events.Add(he);
            cargo.DeriveDeliveryProgress(new HandlingHistory(events));

            return cargo;
        }

        [Test]
        private Cargo PopulateCargoClaimedMelbourne()
        {
            Cargo cargo = PopulateCargoOffMelbourne();

            events.Add(new HandlingEvent(cargo, GetDate("2007-12-09"), DateTime.Now, HandlingType.CLAIM,
                                         SampleLocations.MELBOURNE));
            cargo.DeriveDeliveryProgress(new HandlingHistory(events));

            return cargo;
        }

        [Test]
        private Cargo PopulateCargoOffHongKong()
        {
            Cargo cargo = new Cargo(new TrackingId("XYZ"),
                                    new RouteSpecification(SampleLocations.STOCKHOLM, SampleLocations.MELBOURNE,
                                                           DateTime.Now));


            events.Add(new HandlingEvent(cargo, GetDate("2007-12-01"), DateTime.Now, HandlingType.LOAD,
                                         SampleLocations.STOCKHOLM, voyage));
            events.Add(new HandlingEvent(cargo, GetDate("2007-12-02"), DateTime.Now, HandlingType.UNLOAD,
                                         SampleLocations.HAMBURG, voyage));

            events.Add(new HandlingEvent(cargo, GetDate("2007-12-03"), DateTime.Now, HandlingType.LOAD,
                                         SampleLocations.HAMBURG, voyage));
            events.Add(new HandlingEvent(cargo, GetDate("2007-12-04"), DateTime.Now, HandlingType.UNLOAD,
                                         SampleLocations.HONGKONG, voyage));

            cargo.DeriveDeliveryProgress(new HandlingHistory(events));
            return cargo;
        }

        [Test]
        private Cargo PopulateCargoOnHamburg()
        {
            Cargo cargo = new Cargo(new TrackingId("XYZ"),
                                    new RouteSpecification(SampleLocations.STOCKHOLM, SampleLocations.MELBOURNE,
                                                           DateTime.Now));

            events.Add(new HandlingEvent(cargo, GetDate("2007-12-01"), DateTime.Now, HandlingType.LOAD,
                                         SampleLocations.STOCKHOLM, voyage));
            events.Add(new HandlingEvent(cargo, GetDate("2007-12-02"), DateTime.Now, HandlingType.UNLOAD,
                                         SampleLocations.HAMBURG, voyage));
            events.Add(new HandlingEvent(cargo, GetDate("2007-12-03"), DateTime.Now, HandlingType.LOAD,
                                         SampleLocations.HAMBURG, voyage));

            cargo.DeriveDeliveryProgress(new HandlingHistory(events));
            return cargo;
        }

        [Test]
        private Cargo PopulateCargoOffMelbourne()
        {
            Cargo cargo = new Cargo(new TrackingId("XYZ"),
                                    new RouteSpecification(SampleLocations.STOCKHOLM, SampleLocations.MELBOURNE,
                                                           DateTime.Now));

            events.Add(new HandlingEvent(cargo, GetDate("2007-12-01"), DateTime.Now, HandlingType.LOAD,
                                         SampleLocations.STOCKHOLM, voyage));
            events.Add(new HandlingEvent(cargo, GetDate("2007-12-02"), DateTime.Now, HandlingType.UNLOAD,
                                         SampleLocations.HAMBURG, voyage));

            events.Add(new HandlingEvent(cargo, GetDate("2007-12-03"), DateTime.Now, HandlingType.LOAD,
                                         SampleLocations.HAMBURG, voyage));
            events.Add(new HandlingEvent(cargo, GetDate("2007-12-04"), DateTime.Now, HandlingType.UNLOAD,
                                         SampleLocations.HONGKONG, voyage));

            events.Add(new HandlingEvent(cargo, GetDate("2007-12-05"), DateTime.Now, HandlingType.LOAD,
                                         SampleLocations.HONGKONG, voyage));
            events.Add(new HandlingEvent(cargo, GetDate("2007-12-07"), DateTime.Now, HandlingType.UNLOAD,
                                         SampleLocations.MELBOURNE, voyage));

            cargo.DeriveDeliveryProgress(new HandlingHistory(events));
            return cargo;
        }
               
        [Test]
        public void TestIsMisdirected()
        {
            //A cargo with no itinerary is not misdirected
            Cargo cargo = new Cargo(new TrackingId("TRKID"),
                                    new RouteSpecification(SampleLocations.SHANGHAI, SampleLocations.GOTHENBURG,
                                                           DateTime.Now));
            Assert.IsFalse(cargo.Delivery.IsMisdirected);

            cargo = SetUpCargoWithItinerary(SampleLocations.SHANGHAI, SampleLocations.ROTTERDAM,
                                            SampleLocations.GOTHENBURG);

            //A cargo with no handling events is not misdirected
            Assert.IsFalse(cargo.Delivery.IsMisdirected);

            var handlingEvents = new Collection<HandlingEvent>
                                     {
                                         new HandlingEvent(cargo, new DateTime(10), new DateTime(20),
                                                           HandlingType.RECEIVE, SampleLocations.SHANGHAI),

                                         new HandlingEvent(cargo, new DateTime(30), new DateTime(40),
                                                           HandlingType.LOAD, SampleLocations.SHANGHAI, voyage),

                                         new HandlingEvent(cargo, new DateTime(50), new DateTime(60),
                                                           HandlingType.UNLOAD, SampleLocations.ROTTERDAM, voyage),

                                         new HandlingEvent(cargo, new DateTime(70), new DateTime(80),
                                                           HandlingType.LOAD, SampleLocations.ROTTERDAM, voyage),

                                         new HandlingEvent(cargo, new DateTime(90), new DateTime(100),
                                                           HandlingType.UNLOAD, SampleLocations.GOTHENBURG, voyage),

                                         new HandlingEvent(cargo, new DateTime(110), new DateTime(120),
                                                           HandlingType.CLAIM, SampleLocations.GOTHENBURG),

                                         new HandlingEvent(cargo, new DateTime(130), new DateTime(140),
                                                           HandlingType.CUSTOMS, SampleLocations.GOTHENBURG)
                                     };

            //Happy path
            events.AddRange(handlingEvents);
            cargo.DeriveDeliveryProgress(new HandlingHistory(events));
            Assert.IsFalse(cargo.Delivery.IsMisdirected);

            //Try a couple of failing ones
            cargo = SetUpCargoWithItinerary(SampleLocations.SHANGHAI, SampleLocations.ROTTERDAM,
                                            SampleLocations.GOTHENBURG);

            handlingEvents = new Collection<HandlingEvent>
                                 {
                                     new HandlingEvent(cargo, DateTime.Now, DateTime.Now, HandlingType.RECEIVE,
                                                       SampleLocations.HANGZOU)
                                 };

            events.AddRange(handlingEvents);
            cargo.DeriveDeliveryProgress(new HandlingHistory(events));

            Assert.IsTrue(cargo.Delivery.IsMisdirected);


            cargo = SetUpCargoWithItinerary(SampleLocations.SHANGHAI, SampleLocations.ROTTERDAM,
                                            SampleLocations.GOTHENBURG);

            handlingEvents = new Collection<HandlingEvent>
                                 {
                                     new HandlingEvent(cargo, new DateTime(10), new DateTime(20),
                                                       HandlingType.RECEIVE,
                                                       SampleLocations.SHANGHAI),
                                     new HandlingEvent(cargo, new DateTime(30), new DateTime(40),
                                                       HandlingType.LOAD,
                                                       SampleLocations.SHANGHAI, voyage),
                                     new HandlingEvent(cargo, new DateTime(50), new DateTime(60),
                                                       HandlingType.UNLOAD,
                                                       SampleLocations.ROTTERDAM, voyage),
                                     new HandlingEvent(cargo, new DateTime(70), new DateTime(80),
                                                       HandlingType.LOAD,
                                                       SampleLocations.ROTTERDAM, voyage)
                                 };

            events.AddRange(handlingEvents);
            cargo.DeriveDeliveryProgress(new HandlingHistory(events));

            Assert.IsTrue(cargo.Delivery.IsMisdirected);


            cargo = SetUpCargoWithItinerary(SampleLocations.SHANGHAI, SampleLocations.ROTTERDAM,
                                            SampleLocations.GOTHENBURG);
            handlingEvents = new Collection<HandlingEvent>
                                 {
                                     new HandlingEvent(cargo, new DateTime(10), new DateTime(20),
                                                       HandlingType.RECEIVE,
                                                       SampleLocations.SHANGHAI),
                                     new HandlingEvent(cargo, new DateTime(30), new DateTime(40),
                                                       HandlingType.LOAD,
                                                       SampleLocations.SHANGHAI, voyage),
                                     new HandlingEvent(cargo, new DateTime(50), new DateTime(60),
                                                       HandlingType.UNLOAD,
                                                       SampleLocations.ROTTERDAM, voyage),
                                     new HandlingEvent(cargo, DateTime.Now, DateTime.Now, HandlingType.CLAIM,
                                                       SampleLocations.ROTTERDAM)
                                 };

            events.AddRange(handlingEvents);
            cargo.DeriveDeliveryProgress(new HandlingHistory(events));

            Assert.IsTrue(cargo.Delivery.IsMisdirected);
        }

        private Cargo SetUpCargoWithItinerary(Location origin, Location midpoint, Location destination)
        {
            var cargo = new Cargo(new TrackingId("CARGO1"), new RouteSpecification(origin, destination, DateTime.Now));

            var itinerary = new Itinerary(
                new List<Leg>
                    {
                        new Leg(voyage, origin, midpoint, DateTime.Now, DateTime.Now),
                        new Leg(voyage, midpoint, destination, DateTime.Now, DateTime.Now)
                    });

            cargo.AssignToRoute(itinerary);
            return cargo;
        }


        /// <summary>
        /// Parse an ISO 8601 (YYYY-MM-DD) String to Date    
        /// </summary>
        /// <param name="isoFormat">String to parse.</param>
        /// <returns>Created date instance.</returns>
        private static DateTime GetDate(string isoFormat)
        {
            return DateTime.ParseExact(isoFormat, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
    }
}