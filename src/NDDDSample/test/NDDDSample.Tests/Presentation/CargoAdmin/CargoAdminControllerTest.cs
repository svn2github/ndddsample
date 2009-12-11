namespace NDDDSample.Tests.Presentation.CargoAdmin
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Web.Routing;
    using Interfaces.BookingRemoteService.Common;
    using Interfaces.BookingRemoteService.Common.Dto;
    using Moq;
    using NUnit.Framework;
    using Web.Controllers.CargoAdmin;

    #endregion

    [TestFixture, Category(UnitTestCategories.Controllers)]
    public class CargoAdminControllerTest
    {
        [Test]
        public void RegistrationFormTest()
        {
            //Arrange
            var bookingServiceFacadeMock = new Mock<IBookingServiceFacade>();
            IList<LocationDTO> locationDtos = new List<LocationDTO>
                                                  {
                                                      new LocationDTO("unLogcode1", "name1"),
                                                      new LocationDTO("unLogcode2", "name2")
                                                  };
            bookingServiceFacadeMock.Setup(b => b.ListShippingLocations()).Returns(locationDtos);
            IBookingServiceFacade bookingServiceFacade = bookingServiceFacadeMock.Object;
            var controller = new CargoAdminController(bookingServiceFacade);

            //Act
            var viewModel = controller.RegistrationForm()
                .GetModel<RegistrationFormViewModel>();

            //Assert
            bookingServiceFacadeMock.Verify(b => b.ListShippingLocations(), Times.Once(),
                                            "ListShippingLocations is not invoked");
            Assert.AreEqual(viewModel.LocationDtos.Count, locationDtos.Count, "Count of Locations is not the same");
            Assert.AreEqual(viewModel.LocationDtos[0].Name, locationDtos[0].Name, "Location Name doesnt match");
            Assert.AreEqual(viewModel.LocationDtos[0].UnLocode, locationDtos[0].UnLocode, "UnLocode doesnt match");
            Assert.AreEqual(viewModel.UnLoccodes[0], locationDtos[0].UnLocode, "UnLoccodes: UnLocode doesnt match");
        }


        [Test]
        public void RegisterTest()
        {
            //Arrange
            var origin = "or1";
            var destination = "dest1";
            var arrivalDeadline = new DateTime(2000, 2, 2);
            string trackId = "trackId";
            var bookingServiceFacadeMock = new Mock<IBookingServiceFacade>();
            bookingServiceFacadeMock.Setup(b => b.BookNewCargo(origin, destination, arrivalDeadline))
                .Returns(trackId).Verifiable();

            var controller = new Mock<CargoAdminController>(bookingServiceFacadeMock.Object);

            //Act           
            controller.Object.Register(new RegistrationCommand(origin, destination,
                                                               arrivalDeadline.ToString(
                                                                   CargoAdminController.RegisterDateFormat)));

            //Assert            
            controller.Verify(
                m =>
                m.RedirectToAction(It.Is<string>(s => s == CargoAdminController.ShowActionName),
                                   It.IsAny<RouteValueDictionary>()));
            bookingServiceFacadeMock.VerifyAll();
        }

        [Test]
        public void ListTest()
        {
            //Arrange
            var bookingServiceFacadeMock = new Mock<IBookingServiceFacade>();
            IList<CargoRoutingDTO> cargoRoutingDtos = new List<CargoRoutingDTO>()
                                                          {
                                                              new CargoRoutingDTO("trackId", "origin", "finalDest",
                                                                                  new DateTime(2000, 12, 12), false)
                                                          };
            bookingServiceFacadeMock.Setup(m => m.ListAllCargos()).Returns(cargoRoutingDtos).Verifiable();
            var controller = new CargoAdminController(bookingServiceFacadeMock.Object);

            //Act
            var viewModel = controller.List()
                .GetModel<IList<CargoRoutingDTO>>();

            //Assert
            bookingServiceFacadeMock.VerifyAll();
            var cargoRouting = viewModel[0];
            Assert.AreEqual(viewModel.Count, 1);
            Assert.AreEqual(cargoRouting.ArrivalDeadline, new DateTime(2000, 12, 12));
            Assert.AreEqual(cargoRouting.TrackingId, "trackId");
            Assert.AreEqual(cargoRouting.Origin, "origin");
            Assert.AreEqual(cargoRouting.FinalDestination, "finalDest");
            Assert.AreEqual(cargoRouting.IsMisrouted, false);
        }

        [Test]
        public void ShowTest()
        {
            //Arrange
            var bookingServiceFacadeMock = new Mock<IBookingServiceFacade>();
            var controller = new CargoAdminController(bookingServiceFacadeMock.Object);
            string trackingId = "trackId";
            var cargoRoutingDto = new CargoRoutingDTO(trackingId, "origin", "finalDest", new DateTime(2000, 12, 12),
                                                      false);
            bookingServiceFacadeMock.Setup(m => m.LoadCargoForRouting(trackingId)).Returns(cargoRoutingDto).Verifiable();

            //Act
            var viewModel = controller.Show(trackingId)
                .GetModel<CargoRoutingDTO>();

            //Assert
            bookingServiceFacadeMock.Verify();
            Assert.AreEqual(viewModel.ArrivalDeadline, new DateTime(2000, 12, 12));
            Assert.AreEqual(viewModel.TrackingId, trackingId);
            Assert.AreEqual(viewModel.Origin, "origin");
            Assert.AreEqual(viewModel.FinalDestination, "finalDest");
            Assert.AreEqual(viewModel.IsMisrouted, false);
        }

        [Test]
        public void SelectItineraryTest()
        {
            //Arrange
            var bookingServiceFacadeMock = new Mock<IBookingServiceFacade>();
            var controller = new CargoAdminController(bookingServiceFacadeMock.Object);
            string trackingId = "trackId";

            IList<LegDTO> legDtos = new List<LegDTO>()
                                        {
                                            new LegDTO(
                                                "voyageNumber",
                                                "fromPort", "toPort",
                                                new DateTime(2002, 1, 1),
                                                new DateTime(2002, 2, 2))
                                        };

            var cargoRoutingDto = new CargoRoutingDTO(trackingId, "origin", "finalDest",
                                                      new DateTime(2000, 12, 12), false);


            bookingServiceFacadeMock.Setup(m => m.RequestPossibleRoutesForCargo(trackingId))
                .Returns(new List<RouteCandidateDTO>() { new RouteCandidateDTO(legDtos) }).Verifiable();

            bookingServiceFacadeMock.Setup(m => m.LoadCargoForRouting(trackingId))
                .Returns(cargoRoutingDto).Verifiable();

            //Act
            var viewModel = controller.SelectItinerary(trackingId)
                .GetModel<SelectItineraryViewModel>();

            //Assert
            bookingServiceFacadeMock.Verify();
            var modelRouteLeg = viewModel.RouteCandidates[0].Legs[0];
            Assert.AreEqual(modelRouteLeg.VoyageNumber, "voyageNumber");
            Assert.AreEqual(modelRouteLeg.FromLocation, "fromPort");
            Assert.AreEqual(modelRouteLeg.LoadTime, new DateTime(2002, 1, 1));

            Assert.AreEqual(viewModel.Cargo.TrackingId, trackingId);
            Assert.AreEqual(viewModel.Cargo.ArrivalDeadline, new DateTime(2000, 12, 12));
            Assert.AreEqual(viewModel.Cargo.Origin, "origin");
        }
    }
}