namespace NDDDSample.Tests.Domain.Model.Cargos
{
    #region Usings

    using System.Collections.Generic;
    using Application.Utils;
    using NDDDSample.Domain.Model.Cargos;
    using NDDDSample.Domain.Model.Locations;
    using NDDDSample.Domain.Model.Voyages;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class RouteSpecificationTest
    {
        private static readonly Voyage hongKongTokyoNewYork = new Voyage.Builder(
            new VoyageNumber("V001"), SampleLocations.HONGKONG).
            AddMovement(SampleLocations.TOKYO, DateTestUtil.toDate("2009-02-01"), DateTestUtil.toDate("2009-02-05")).
            AddMovement(SampleLocations.NEWYORK, DateTestUtil.toDate("2009-02-06"), DateTestUtil.toDate("2009-02-10")).
            AddMovement(SampleLocations.HONGKONG, DateTestUtil.toDate("2009-02-11"), DateTestUtil.toDate("2009-02-14")).
            Build();

        private static readonly Voyage dallasNewYorkChicago = new Voyage.Builder(
            new VoyageNumber("V002"), SampleLocations.DALLAS).
            AddMovement(SampleLocations.NEWYORK, DateTestUtil.toDate("2009-02-06"), DateTestUtil.toDate("2009-02-07")).
            AddMovement(SampleLocations.CHICAGO, DateTestUtil.toDate("2009-02-12"), DateTestUtil.toDate("2009-02-20")).
            Build();

        // TODO:
        // it shouldn't be possible to create Legs that have load/unload locations
        // and/or dates that don't match the voyage's carrier movements.
        private readonly Itinerary itinerary = new Itinerary(new List<Leg>
                                                                 {
                                                                     new Leg(hongKongTokyoNewYork,
                                                                             SampleLocations.HONGKONG,
                                                                             SampleLocations.NEWYORK,
                                                                             DateTestUtil.toDate("2009-02-01"),
                                                                             DateTestUtil.toDate("2009-02-10")),
                                                                     new Leg(dallasNewYorkChicago,
                                                                             SampleLocations.NEWYORK,
                                                                             SampleLocations.CHICAGO,
                                                                             DateTestUtil.toDate("2009-02-12"),
                                                                             DateTestUtil.toDate("2009-02-20"))
                                                                 });

        [Test]
        public void testIsSatisfiedBy_Success()
        {
            var routeSpecification = new RouteSpecification(
                SampleLocations.HONGKONG, SampleLocations.CHICAGO, DateTestUtil.toDate("2009-03-01"));

            Assert.IsTrue(routeSpecification.IsSatisfiedBy(itinerary));
        }

        [Test]
        public void testIsSatisfiedBy_WrongOrigin()
        {
            var routeSpecification = new RouteSpecification(
                SampleLocations.HANGZOU, SampleLocations.CHICAGO, DateTestUtil.toDate("2009-03-01"));

            Assert.IsFalse(routeSpecification.IsSatisfiedBy(itinerary));
        }

        [Test]
        public void testIsSatisfiedBy_WrongDestination()
        {
            var routeSpecification = new RouteSpecification(
                SampleLocations.HONGKONG, SampleLocations.DALLAS, DateTestUtil.toDate("2009-03-01"));

            Assert.IsFalse(routeSpecification.IsSatisfiedBy(itinerary));
        }

        [Test]
        public void testIsSatisfiedBy_MissedDeadline()
        {
            var routeSpecification = new RouteSpecification(
                SampleLocations.HONGKONG, SampleLocations.CHICAGO, DateTestUtil.toDate("2009-02-15"));

            Assert.IsFalse(routeSpecification.IsSatisfiedBy(itinerary));
        }
    }
}