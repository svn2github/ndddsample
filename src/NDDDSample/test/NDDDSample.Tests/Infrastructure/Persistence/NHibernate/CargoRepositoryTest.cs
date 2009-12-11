namespace NDDDSample.Tests.Infrastructure.Persistence.NHibernate
{
    #region Usings

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using NDDDSample.Domain.Model.Cargos;
    using NDDDSample.Domain.Model.Handlings;
    using NDDDSample.Domain.Model.Locations;
    using NDDDSample.Domain.Model.Voyages;
    using NDDDSample.Persistence.NHibernate;
    using NDDDSample.Persistence.NHibernate.Utils;
    using NUnit.Framework;
    using Rhino.Commons;

    #endregion

    [TestFixture, Category(UnitTestCategories.Infrastructure)]
    public class CargoRepositoryTest : BaseRepositoryTest
    {
        private ICargoRepository cargoRepository;
        private ILocationRepository locationRepository;
        private IVoyageRepository voyageRepository;
        private IHandlingEventRepository handlingEventRepository;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            cargoRepository = new CargoRepositoryHibernate();
            locationRepository = new LocationRepositoryHibernate();
            voyageRepository = new VoyageRepositoryHibernate();
            handlingEventRepository = new HandlingEventRepositoryHibernate();
        }

        [Test]
        public void FindByCargoId()
        {
            TrackingId trackingId = new TrackingId("FGH");
            Cargo cargo = cargoRepository.Find(trackingId);
            Assert.AreEqual(SampleLocations.STOCKHOLM, cargo.Origin);
            Assert.AreEqual(SampleLocations.HONGKONG, cargo.RouteSpecification.Origin);
            Assert.AreEqual(SampleLocations.HELSINKI, cargo.RouteSpecification.Destination);

            Assert.IsNotNull(cargo.Delivery);

            IList<HandlingEvent> events =
                handlingEventRepository.LookupHandlingHistoryOfCargo(trackingId).DistinctEventsByCompletionTime();
            Assert.AreEqual(2, events.Count);

            HandlingEvent firstEvent = events[0];
            AssertHandlingEvent(cargo, firstEvent, HandlingType.RECEIVE, SampleLocations.HONGKONG, 100, 160, Voyage.NONE);

            HandlingEvent secondEvent = events[1];

            Voyage hongkongMelbourneTokyoAndBack = new Voyage.Builder(
                new VoyageNumber("0303"), SampleLocations.HONGKONG).
                AddMovement(SampleLocations.MELBOURNE, new DateTime(), new DateTime()).
                AddMovement(SampleLocations.TOKYO, new DateTime(), new DateTime()).
                AddMovement(SampleLocations.HONGKONG, new DateTime(), new DateTime()).
                Build();

            AssertHandlingEvent(cargo, secondEvent, HandlingType.LOAD, SampleLocations.HONGKONG, 150, 110,
                                hongkongMelbourneTokyoAndBack);

            IList<Leg> legs = cargo.Itinerary.Legs;
            Assert.AreEqual(3, legs.Count);

            Leg firstLeg = legs[0];
            AssertLeg(firstLeg, "0101", SampleLocations.HONGKONG, SampleLocations.MELBOURNE);

            Leg secondLeg = legs[1];
            AssertLeg(secondLeg, "0101", SampleLocations.MELBOURNE, SampleLocations.STOCKHOLM);

            Leg thirdLeg = legs[2];
            AssertLeg(thirdLeg, "0101", SampleLocations.STOCKHOLM, SampleLocations.HELSINKI);
        }

        private void AssertHandlingEvent(Cargo cargo, HandlingEvent evnt, HandlingType expectedEventType,
                                         Location expectedLocation, int completionTimeMs, int registrationTimeMs,
                                         Voyage voyage)
        {
            Assert.AreEqual(expectedEventType, evnt.Type);
            Assert.AreEqual(expectedLocation, evnt.Location);

            DateTime expectedCompletionTime = SampleDataGenerator.Offset(completionTimeMs);
            Assert.AreEqual(expectedCompletionTime, evnt.CompletionTime);

            DateTime expectedRegistrationTime = SampleDataGenerator.Offset(registrationTimeMs);
            Assert.AreEqual(expectedRegistrationTime, evnt.RegistrationTime);

            Assert.AreEqual(voyage, evnt.Voyage);
            Assert.AreEqual(cargo, evnt.Cargo);
        }


        private void AssertLeg(Leg firstLeg, String vn, Location expectedFrom, Location expectedTo)
        {
            Assert.AreEqual(new VoyageNumber(vn), firstLeg.Voyage.VoyageNumber);
            Assert.AreEqual(expectedFrom, firstLeg.LoadLocation);
            Assert.AreEqual(expectedTo, firstLeg.UnloadLocation);
        }

        [Test]
        public void FindByCargoIdUnknownId()
        {
            Assert.IsNull(cargoRepository.Find(new TrackingId("UNKNOWN")));
        }

        [Test]
        public void Save()
        {
            //TODO: atrosin make the method transactional because it modifies data
            TrackingId trackingId = new TrackingId("AAA");
            Location origin = locationRepository.Find(SampleLocations.STOCKHOLM.UnLocode);
            Location destination = locationRepository.Find(SampleLocations.MELBOURNE.UnLocode);

            Cargo cargo = new Cargo(trackingId, new RouteSpecification(origin, destination, new DateTime()));
            cargoRepository.Store(cargo);

            cargo.AssignToRoute(new Itinerary(
                                    new List<Leg>
                                        {
                                            new Leg(
                                                voyageRepository.Find(new VoyageNumber("0101")),
                                                locationRepository.Find(SampleLocations.STOCKHOLM.UnLocode),
                                                locationRepository.Find(SampleLocations.MELBOURNE.UnLocode),
                                                new DateTime(), new DateTime())
                                        }));

            Flush();


            IList cargoListFromDb = GetPlainCargoListFromDbByTrackId(trackingId);

            Assert.AreEqual("AAA", cargoListFromDb[0] /*TRACKING_ID*/);

            int originId = GetIntId(origin);
            Assert.AreEqual(originId, cargoListFromDb[1] /*SPEC_ORIGIN_ID*/);

            int destinationId = GetIntId(destination);
            Assert.AreEqual(destinationId, cargoListFromDb[2] /*SPEC_DESTINATION_ID*/);

            UnitOfWork.CurrentSession.Clear();

            Cargo loadedCargo = cargoRepository.Find(trackingId);
            Assert.AreEqual(1, loadedCargo.Itinerary.Legs.Count);
        }


        [Test]
        public void ReplaceItinerary()
        {
            Cargo cargo = cargoRepository.Find(new TrackingId("FGH"));

            Assert.AreEqual(3L, GetLegCountFromDbByCargoId(cargo));

            Location legFrom = locationRepository.Find(new UnLocode("FIHEL"));
            Location legTo = locationRepository.Find(new UnLocode("DEHAM"));
            Itinerary newItinerary =
                new Itinerary(new List<Leg>
                                  {(new Leg(SampleVoyages.CM004, legFrom, legTo, new DateTime(), new DateTime()))});

            cargo.AssignToRoute(newItinerary);

            cargoRepository.Store(cargo);
            Flush();

            Assert.AreEqual(1L, GetLegCountFromDbByCargoId(cargo));
        }


        [Test]
        public void FindAll()
        {
            IList<Cargo> all = cargoRepository.FindAll();
            Assert.IsNotNull(all);
            Assert.AreEqual(6, all.Count);
        }

        [Test]
        public void NextTrackingId()
        {
            TrackingId trackingId = cargoRepository.NextTrackingId();
            Assert.IsNotNull(trackingId);

            TrackingId trackingId2 = cargoRepository.NextTrackingId();
            Assert.IsNotNull(trackingId2);
            Assert.IsFalse(trackingId.Equals(trackingId2));
        }

        private static IList GetPlainCargoListFromDbByTrackId(TrackingId trackingId)
        {
            return
                UnitOfWork.CurrentSession.CreateSQLQuery(
                    "select TRACKING_ID, SPEC_ORIGIN_ID, SPEC_DESTINATION_ID from Cargo where tracking_id = ?")
                    .SetString(0, trackingId.IdString)
                    .List()[0] as object[];
        }

        private static long GetLegCountFromDbByCargoId(Cargo cargo)
        {
            return UnitOfWork.CurrentSession.CreateSQLQuery(
                "select count(*) from Leg where cargo_id = ?")
                .SetInt32(0, GetIntId(cargo))
                .UniqueResult<long>();
        }
    }
}