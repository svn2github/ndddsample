namespace NDDDSample.Tests.Infrastructure.Routing
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using Interfaces.PathfinderRemoteService;
    using Interfaces.PathfinderRemoteService.Common;
    using Moq;
    using NDDDSample.Domain.Model.Cargos;
    using NDDDSample.Domain.Model.Locations;
    using NDDDSample.Domain.Model.Voyages;
    using NDDDSample.Infrastructure.ExternalRouting;
    using NDDDSample.Infrastructure.Utils;
    using NUnit.Framework;
    using Persistence.Inmemory;

    #endregion

    [TestFixture, Category(UnitTestCategories.Infrastructure)]
    public class ExternalRoutingServiceTest
    {
        private ExternalRoutingService externalRoutingService;
        private IVoyageRepository voyageRepository;
        private Mock<IVoyageRepository> voyageRepositoryMock;


        [SetUp]
        public void SetUp()
        {
            ILocationRepository locationRepository = new LocationRepositoryInMem();

            voyageRepositoryMock = new Mock<IVoyageRepository>();
            voyageRepository = voyageRepositoryMock.Object;
            var daoTest = new Mock<GraphDAO>();
            daoTest.CallBase = true;
            daoTest.Setup(d => d.ListLocations()).Returns(new List<string>()
                                                              {
                                                                  SampleLocations.TOKYO.UnLocode.IdString,
                                                                  SampleLocations.STOCKHOLM.UnLocode.IdString,
                                                                  SampleLocations.GOTHENBURG.UnLocode.IdString
                                                              });


            IGraphTraversalService graphTraversalService = new GraphTraversalService(daoTest.Object);

            externalRoutingService = new ExternalRoutingService(graphTraversalService, locationRepository,
                                                                voyageRepository);
        }


        [Test]
        public void TestCalculatePossibleRoutes()
        {
            var trackingId = new TrackingId("ABC");
            var routeSpecification = new RouteSpecification(SampleLocations.HONGKONG,
                                                                           SampleLocations.HELSINKI, DateTime.Now);
            var cargo = new Cargo(trackingId, routeSpecification);

            voyageRepositoryMock.Setup(v => v.Find(It.IsAny<VoyageNumber>())).Returns(SampleVoyages.CM002);

            IList<Itinerary> candidates = externalRoutingService.FetchRoutesForSpecification(routeSpecification);
            Assert.IsNotNull(candidates);

            foreach (Itinerary itinerary in candidates)
            {
                IList<Leg> legs = itinerary.Legs;
                Assert.IsNotNull(legs);
                Assert.IsFalse(legs.IsEmpty());

                // Cargo origin and start of first leg should match
                Assert.AreEqual(cargo.Origin, legs[0].LoadLocation);

                // Cargo final destination and last leg stop should match
                Location lastLegStop = legs[legs.Count - 1].UnloadLocation;
                Assert.AreEqual(cargo.RouteSpecification.Destination, lastLegStop);

                for (int i = 0; i < legs.Count - 1; i++)
                {
                    // Assert that all legs are connected
                    Assert.AreEqual(legs[i].UnloadLocation, legs[i + 1].LoadLocation);
                }
            }

            voyageRepositoryMock.Verify();
        }
    }
}