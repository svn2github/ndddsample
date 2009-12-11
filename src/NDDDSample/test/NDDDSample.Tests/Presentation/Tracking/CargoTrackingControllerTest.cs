namespace NDDDSample.Tests.Presentation.Tracking
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using Moq;
    using NDDDSample.Domain.Model.Cargos;
    using NDDDSample.Domain.Model.Handlings;
    using NDDDSample.Domain.Model.Locations;
    using NUnit.Framework;
    using Web.Controllers.Tracking;

    #endregion

    [TestFixture, Category(UnitTestCategories.Controllers)]
    public class CargoTrackingControllerTest
    {
        [Test]
        public void SearchNoExistingCargoTest()
        {
            //Arrange
            var cargoRepositoryMock = new Mock<ICargoRepository>();
            var handlingEventRepositoryMock = new Mock<IHandlingEventRepository>();
            var controllerMock = new Mock<CargoTrackingController>(cargoRepositoryMock.Object,
                                                                   handlingEventRepositoryMock.Object);
            var controller = controllerMock.Object;

            const string trackingIdStr = "trackId";
            var trackingId = new TrackingId(trackingIdStr);
            cargoRepositoryMock.Setup(m => m.Find(trackingId)).Returns((Cargo)null).Verifiable();

            //Act
            var viewModel = controller.Search(new TrackCommand { TrackingId = trackingIdStr })
                .GetModel<CargoTrackingViewAdapter>();

            //Assert
            //Verify expected method calls
            cargoRepositoryMock.Verify();
            handlingEventRepositoryMock.Verify();
            Assert.IsNull(viewModel);
            //Verfy if warning message is set to UI
            controllerMock.Verify(m => m.SetMessage(It.Is<string>(s => s == CargoTrackingController.UnknownMessageId)), Times.Once());
            //Verify that the method is not invoked
            handlingEventRepositoryMock.Verify(m => m.LookupHandlingHistoryOfCargo(It.IsAny<TrackingId>()), Times.Never());
        }

        [Test]
        public void SearchExistingCargoTest()
        {
            //Arrange
            var cargoRepositoryMock = new Mock<ICargoRepository>();
            var handlingEventRepositoryMock = new Mock<IHandlingEventRepository>();
            var controller = new CargoTrackingController(cargoRepositoryMock.Object,
                                                         handlingEventRepositoryMock.Object);

            const string trackingIdStr = "trackId";
            var trackingId = new TrackingId(trackingIdStr);
            var cargo = new Cargo(trackingId,
                                  new RouteSpecification(SampleLocations.STOCKHOLM, SampleLocations.MELBOURNE,
                                                         new DateTime(1983, 06, 12)));

            IList<HandlingEvent> events = new List<HandlingEvent>
                                              {
                                                  new HandlingEvent(cargo, new DateTime(2007, 12, 09), DateTime.Now,
                                                                    HandlingType.CLAIM,
                                                                    SampleLocations.MELBOURNE)
                                              };
            var handlingHistory = new HandlingHistory(events);

            cargoRepositoryMock.Setup(m => m.Find(trackingId)).Returns(cargo).Verifiable();
            handlingEventRepositoryMock.Setup(m => m.LookupHandlingHistoryOfCargo(trackingId)).Returns(handlingHistory);

            //Act
            var viewModel = controller.Search(new TrackCommand {TrackingId = trackingIdStr})
                .GetModel<CargoTrackingViewAdapter>();

            //Assert
            //Verify expected method calls
            cargoRepositoryMock.Verify();
            handlingEventRepositoryMock.Verify();
            //Verify if view Model is set correctly by Controller            
            Assert.AreEqual(viewModel.TrackingId, trackingId.IdString);
            Assert.AreEqual(viewModel.Origin, cargo.Origin.Name);
            Assert.AreEqual(viewModel.Destination, cargo.RouteSpecification.Destination.Name);
            Assert.AreEqual(viewModel.Events[0].Type, events[0].Type.DisplayName);
            Assert.AreEqual(viewModel.Events[0].Location, events[0].Location.Name);
            Assert.AreEqual(viewModel.Events[0].Time, events[0].CompletionTime.ToString(HandlingEventViewAdapter.FORMAT));
        }

    }
}