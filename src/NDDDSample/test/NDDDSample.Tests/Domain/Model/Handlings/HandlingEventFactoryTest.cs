namespace NDDDSample.Tests.Domain.Model.Handlings
{
    #region Usings

    using System;
    using Infrastructure.Persistence.Inmemory;
    using Moq;
    using NDDDSample.Domain.Model.Cargos;
    using NDDDSample.Domain.Model.Handlings;
    using NDDDSample.Domain.Model.Handlings.Exceptions;
    using NDDDSample.Domain.Model.Locations;
    using NDDDSample.Domain.Model.Voyages;
    using NDDDSample.Infrastructure.Utils;
    using NUnit.Framework;

    #endregion

    [TestFixture, Category(UnitTestCategories.DomainModel)]
    public class HandlingEventFactoryTest
    {
        private HandlingEventFactory factory;
        private ICargoRepository cargoRepository;
        private IVoyageRepository voyageRepository;
        private ILocationRepository locationRepository;
        private TrackingId trackingId;
        private Cargo cargo;
        private Mock<ICargoRepository> cargoRepositoryMock;

        [SetUp]
        protected void SetUp()
        {
            cargoRepositoryMock = new Mock<ICargoRepository>();
            cargoRepository = cargoRepositoryMock.Object;
            voyageRepository = new VoyageRepositoryInMem();
            locationRepository = new LocationRepositoryInMem();
            factory = new HandlingEventFactory(cargoRepository, voyageRepository, locationRepository);


            trackingId = new TrackingId("ABC");
            var routeSpecification = new RouteSpecification(SampleLocations.TOKYO,
                                                            SampleLocations.HELSINKI, DateTime.Now);
            cargo = new Cargo(trackingId, routeSpecification);
        }

        [Test]
        public void TestCreateHandlingEventWithCarrierMovement()
        {
            cargoRepositoryMock.Setup(rep => rep.Find(trackingId)).Returns(cargo);

            VoyageNumber voyageNumber = SampleVoyages.CM001.VoyageNumber;
            UnLocode unLocode = SampleLocations.STOCKHOLM.UnLocode;
            DateTime completionTime = DateTime.Now.AddDays(10);
            HandlingEvent handlingEvent = factory.CreateHandlingEvent(
                DateTime.Now, completionTime, trackingId, voyageNumber, unLocode, HandlingType.LOAD
                );

            Assert.IsNotNull(handlingEvent);
            Assert.AreEqual(SampleLocations.STOCKHOLM, handlingEvent.Location);
            Assert.AreEqual(SampleVoyages.CM001, handlingEvent.Voyage);
            Assert.AreEqual(cargo, handlingEvent.Cargo);
            Assert.AreEqual(completionTime, handlingEvent.CompletionTime);
            Assert.IsTrue(handlingEvent.RegistrationTime.Before(DateTime.Now.AddMinutes(1)));
        }

        [Test]
        public void TestCreateHandlingEventWithoutCarrierMovement()
        {
            cargoRepositoryMock.Setup(rep => rep.Find(trackingId)).Returns(cargo);


            UnLocode unLocode = SampleLocations.STOCKHOLM.UnLocode;
            DateTime completionTime = DateTime.Now.AddDays(10);
            HandlingEvent handlingEvent = factory.CreateHandlingEvent(
                DateTime.Now, completionTime, trackingId, null, unLocode, HandlingType.CLAIM
                );

            Assert.IsNotNull(handlingEvent);
            Assert.AreEqual(SampleLocations.STOCKHOLM, handlingEvent.Location);
            Assert.AreEqual(Voyage.NONE, handlingEvent.Voyage);
            Assert.AreEqual(cargo, handlingEvent.Cargo);
            Assert.AreEqual(completionTime, handlingEvent.CompletionTime);
            Assert.IsTrue(handlingEvent.RegistrationTime.Before(DateTime.Now.AddMinutes(1)));
        }

        [Test]
        public void TestCreateHandlingEventUnknownLocation()
        {
            cargoRepositoryMock.Setup(rep => rep.Find(trackingId)).Returns(cargo);

            UnLocode invalid = new UnLocode("NOEXT");
            try
            {
                DateTime completionTime = DateTime.Now.AddDays(10);
                factory.CreateHandlingEvent(
                    DateTime.Now, completionTime, trackingId, SampleVoyages.CM001.VoyageNumber, invalid,
                    HandlingType.LOAD
                    );
                Assert.Fail("Expected UnknownLocationException");
            }
            catch (UnknownLocationException expected) {}
        }

        [Test]
        public void TestCreateHandlingEventUnknownCarrierMovement()
        {
            cargoRepositoryMock.Setup(rep => rep.Find(trackingId)).Returns(cargo);

            try
            {
                var invalid = new VoyageNumber("XXX");
                DateTime completionTime = DateTime.Now.AddDays(10);
                factory.CreateHandlingEvent(
                    DateTime.Now, completionTime, trackingId, invalid, SampleLocations.STOCKHOLM.UnLocode,
                    HandlingType.LOAD
                    );
                Assert.Fail("Expected UnknownVoyageException");
            }
            catch (UnknownVoyageException expected) {}
        }

        [Test]
        public void TestCreateHandlingEventUnknownTrackingId()
        {
            cargoRepositoryMock.Setup(rep => rep.Find(trackingId)).Returns((Cargo)null);

            try
            {
                DateTime completionTime = DateTime.Now.AddDays(10);
                factory.CreateHandlingEvent(
                    DateTime.Now, completionTime, trackingId, SampleVoyages.CM001.VoyageNumber,
                    SampleLocations.STOCKHOLM.UnLocode, HandlingType.LOAD
                    );
                Assert.Fail("Expected UnknownCargoException");
            }
            catch (UnknownCargoException expected) {}
        }

        [TearDown]
        protected void TearDown()
        {
            cargoRepositoryMock.Verify();
        }
    }
}