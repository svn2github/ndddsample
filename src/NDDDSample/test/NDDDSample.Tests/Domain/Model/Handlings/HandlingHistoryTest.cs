namespace NDDDSample.Tests.Domain.Model.Handlings
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using NDDDSample.Domain.Model.Cargos;
    using NDDDSample.Domain.Model.Handlings;
    using NDDDSample.Domain.Model.Voyages;    
    using NUnit.Framework;
    using NDDDSample.Domain.Model.Locations;

    #endregion

    [TestFixture, Category(UnitTestCategories.DomainModel)]
    public class HandlingHistoryTest
    {
        Cargo cargo;
        Voyage voyage;
        HandlingEvent event1;
        HandlingEvent event1duplicate;
        HandlingEvent event2;
        HandlingHistory handlingHistory;

        [SetUp]
        protected void SetUp()
        {
            cargo = new Cargo(new TrackingId("ABC"), new RouteSpecification(SampleLocations.SHANGHAI, SampleLocations.DALLAS, DateTime.Parse("2009-04-01")));
            voyage = new Voyage.Builder(new VoyageNumber("X25"), SampleLocations.HONGKONG).
              AddMovement(SampleLocations.SHANGHAI, new DateTime(), new DateTime()).
              AddMovement(SampleLocations.DALLAS, new DateTime(), new DateTime()).
              Build();
            event1 = new HandlingEvent(cargo, DateTime.Parse("2009-03-05"), new DateTime(100), HandlingType.LOAD, SampleLocations.SHANGHAI, voyage);
            event1duplicate = new HandlingEvent(cargo, DateTime.Parse("2009-03-05"), new DateTime(200), HandlingType.LOAD, SampleLocations.SHANGHAI, voyage);
            event2 = new HandlingEvent(cargo, DateTime.Parse("2009-03-10"), new DateTime(150), HandlingType.UNLOAD, SampleLocations.DALLAS, voyage);

            handlingHistory = new HandlingHistory(new List<HandlingEvent>{event2, event1, event1duplicate});
        }

        [Test]
        public void TestDistinctEventsByCompletionTime()
        {
            Assert.AreEqual(new List<HandlingEvent>{event1, event2}, handlingHistory.DistinctEventsByCompletionTime());
        }

        [Test]
        public void TestMostRecentlyCompletedEvent()
        {
            Assert.AreEqual(event2, handlingHistory.MostRecentlyCompletedEvent());
        }
    }
}