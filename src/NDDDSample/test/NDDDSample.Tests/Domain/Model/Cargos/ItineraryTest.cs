namespace NDDDSample.Tests.Domain.Model.Cargos
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using NDDDSample.Domain.Model.Cargos;
    using NDDDSample.Domain.Model.Handlings;
    using NDDDSample.Domain.Model.Locations;
    using NDDDSample.Domain.Model.Voyages;
    using NUnit.Framework;

    #endregion

    [TestFixture, Category(UnitTestCategories.DomainModel)]
    public class ItineraryTest
    {
        private static readonly DateTime dateTime = new DateTime(2009, 1, 2);        
        private Voyage voyage, wrongVoyage;

        [SetUp]
        protected void SetUp()
        {
            voyage = new Voyage.Builder(new VoyageNumber("0123"), SampleLocations.SHANGHAI).
                AddMovement(SampleLocations.ROTTERDAM, dateTime, dateTime).
                AddMovement(SampleLocations.GOTHENBURG, dateTime, dateTime).
                Build();

            wrongVoyage = new Voyage.Builder(new VoyageNumber("666"), SampleLocations.NEWYORK).
                AddMovement(SampleLocations.STOCKHOLM, dateTime, dateTime).
                AddMovement(SampleLocations.HELSINKI, dateTime, dateTime).
                Build();
        }

        [Test]
        public void TestCargoOnTrack()
        {
            var trackingId = new TrackingId("CARGO1");
            var routeSpecification = new RouteSpecification(SampleLocations.SHANGHAI,
                                                                           SampleLocations.GOTHENBURG, dateTime);
            var cargo = new Cargo(trackingId, routeSpecification);

            var itinerary = new Itinerary(
                new List<Leg>
                    {
                        new Leg(voyage, SampleLocations.SHANGHAI, SampleLocations.ROTTERDAM, dateTime, dateTime),
                        new Leg(voyage, SampleLocations.ROTTERDAM, SampleLocations.GOTHENBURG, dateTime, dateTime)
                    });

            //Happy path
            var evnt = new HandlingEvent(cargo, dateTime, dateTime, HandlingType.RECEIVE,
                                                   SampleLocations.SHANGHAI);
            Assert.IsTrue(itinerary.IsExpected(evnt));

            evnt = new HandlingEvent(cargo, dateTime, dateTime, HandlingType.LOAD, SampleLocations.SHANGHAI, voyage);
            Assert.IsTrue(itinerary.IsExpected(evnt));

            evnt = new HandlingEvent(cargo, dateTime, dateTime, HandlingType.UNLOAD, SampleLocations.ROTTERDAM, voyage);
            Assert.IsTrue(itinerary.IsExpected(evnt));

            evnt = new HandlingEvent(cargo, dateTime, dateTime, HandlingType.LOAD, SampleLocations.ROTTERDAM, voyage);
            Assert.IsTrue(itinerary.IsExpected(evnt));

            evnt = new HandlingEvent(cargo, dateTime, dateTime, HandlingType.UNLOAD, SampleLocations.GOTHENBURG, voyage);
            Assert.IsTrue(itinerary.IsExpected(evnt));

            evnt = new HandlingEvent(cargo, dateTime, dateTime, HandlingType.CLAIM, SampleLocations.GOTHENBURG);
            Assert.IsTrue(itinerary.IsExpected(evnt));

            //Customs evnt changes nothing
            evnt = new HandlingEvent(cargo, dateTime, dateTime, HandlingType.CUSTOMS, SampleLocations.GOTHENBURG);
            Assert.IsTrue(itinerary.IsExpected(evnt));

            //Received at the wrong location
            evnt = new HandlingEvent(cargo, dateTime, dateTime, HandlingType.RECEIVE, SampleLocations.HANGZOU);
            Assert.IsFalse(itinerary.IsExpected(evnt));

            //Loaded to onto the wrong ship, correct location
            evnt = new HandlingEvent(cargo, dateTime, dateTime, HandlingType.LOAD, SampleLocations.ROTTERDAM,
                                     wrongVoyage);
            Assert.IsFalse(itinerary.IsExpected(evnt));

            //Unloaded from the wrong ship in the wrong location
            evnt = new HandlingEvent(cargo, dateTime, dateTime, HandlingType.UNLOAD, SampleLocations.HELSINKI,
                                     wrongVoyage);
            Assert.IsFalse(itinerary.IsExpected(evnt));

            evnt = new HandlingEvent(cargo, dateTime, dateTime, HandlingType.CLAIM, SampleLocations.ROTTERDAM);
            Assert.IsFalse(itinerary.IsExpected(evnt));
        }

        [Test]
        public void TestNextExpectedEvent()
        {
            //TODO: implement the test
        }

        [Test]    
        public void TestCreateItinerary()
        {
            try
            {
                new Itinerary(new List<Leg>());
                Assert.Fail("An empty itinerary is not OK");
            }
            catch (Exception iae)
            {
                //Expected
            }

            try
            {
                List<Leg> legs = null;
                new Itinerary(legs);
                Assert.Fail("Null itinerary is not OK");
            }
            catch (Exception iae)
            {
                //Expected
            }
        }
    }
}