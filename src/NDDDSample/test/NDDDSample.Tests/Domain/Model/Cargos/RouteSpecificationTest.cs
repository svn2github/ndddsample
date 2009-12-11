namespace NDDDSample.Tests.Domain.Model.Cargos
{
    #region Usings

    using System.Collections.Generic;
    using NDDDSample.Domain.Model.Cargos;
    using NDDDSample.Domain.Model.Locations;
    using NDDDSample.Domain.Model.Voyages;
    using NDDDSample.Infrastructure;
    using NUnit.Framework;

    #endregion

    [TestFixture, Category(UnitTestCategories.DomainModel)]
    public class RouteSpecificationTest
    {
        private static readonly Voyage hongKongTokyoNewYork = new Voyage.Builder(
            new VoyageNumber("V001"), SampleLocations.HONGKONG).
            AddMovement(SampleLocations.TOKYO, DateUtil.ToDate("2009-02-01"), DateUtil.ToDate("2009-02-05")).
            AddMovement(SampleLocations.NEWYORK, DateUtil.ToDate("2009-02-06"), DateUtil.ToDate("2009-02-10")).
            AddMovement(SampleLocations.HONGKONG, DateUtil.ToDate("2009-02-11"), DateUtil.ToDate("2009-02-14")).
            Build();

        private static readonly Voyage dallasNewYorkChicago = new Voyage.Builder(
            new VoyageNumber("V002"), SampleLocations.DALLAS).
            AddMovement(SampleLocations.NEWYORK, DateUtil.ToDate("2009-02-06"), DateUtil.ToDate("2009-02-07")).
            AddMovement(SampleLocations.CHICAGO, DateUtil.ToDate("2009-02-12"), DateUtil.ToDate("2009-02-20")).
            Build();

        // TODO:
        // it shouldn't be possible to create Legs that have load/unload locations
        // and/or dates that don't match the voyage's carrier movements.
        private readonly Itinerary itinerary = new Itinerary(new List<Leg>
                                                                 {
                                                                     new Leg(hongKongTokyoNewYork,
                                                                             SampleLocations.HONGKONG,
                                                                             SampleLocations.NEWYORK,
                                                                             DateUtil.ToDate("2009-02-01"),
                                                                             DateUtil.ToDate("2009-02-10")),
                                                                     new Leg(dallasNewYorkChicago,
                                                                             SampleLocations.NEWYORK,
                                                                             SampleLocations.CHICAGO,
                                                                             DateUtil.ToDate("2009-02-12"),
                                                                             DateUtil.ToDate("2009-02-20"))
                                                                 });

        [Test]
        public void TestIsSatisfiedBySuccess()
        {
            var routeSpecification = new RouteSpecification(
                SampleLocations.HONGKONG, SampleLocations.CHICAGO, DateUtil.ToDate("2009-03-01"));

            Assert.IsTrue(routeSpecification.IsSatisfiedBy(itinerary));
        }

        [Test]
        public void TestIsSatisfiedByWrongOrigin()
        {
            var routeSpecification = new RouteSpecification(
                SampleLocations.HANGZOU, SampleLocations.CHICAGO, DateUtil.ToDate("2009-03-01"));

            Assert.IsFalse(routeSpecification.IsSatisfiedBy(itinerary));
        }

        [Test]
        public void TestIsSatisfiedByWrongDestination()
        {
            var routeSpecification = new RouteSpecification(
                SampleLocations.HONGKONG, SampleLocations.DALLAS, DateUtil.ToDate("2009-03-01"));

            Assert.IsFalse(routeSpecification.IsSatisfiedBy(itinerary));
        }

        [Test]
        public void TestIsSatisfiedByMissedDeadline()
        {
            var routeSpecification = new RouteSpecification(
                SampleLocations.HONGKONG, SampleLocations.CHICAGO, DateUtil.ToDate("2009-02-15"));

            Assert.IsFalse(routeSpecification.IsSatisfiedBy(itinerary));
        }
    }
}