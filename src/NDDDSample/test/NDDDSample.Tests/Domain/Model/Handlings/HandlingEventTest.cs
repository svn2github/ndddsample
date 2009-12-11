namespace NDDDSample.Tests.Domain.Model.Handlings
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using NDDDSample.Domain.Model.Cargos;
    using NDDDSample.Domain.Model.Handlings;
    using NDDDSample.Domain.Model.Voyages;
    using NDDDSample.Domain.Shared;
    using NUnit.Framework;
    using NDDDSample.Domain.Model.Locations;

    #endregion

    [TestFixture, Category(UnitTestCategories.DomainModel)]  
    public class HandlingEventTest
    {
        Cargo cargo;        

        [SetUp]
        protected void SetUp()
        {
            var trackingId = new TrackingId("XYZ");
            var routeSpecification = new RouteSpecification(SampleLocations.HONGKONG, SampleLocations.NEWYORK, new DateTime());
            cargo = new Cargo(trackingId, routeSpecification);
        }

        [Test]
        public void TestNewWithLocation()
        {
            var e1 = new HandlingEvent(cargo, new DateTime(), new DateTime(), HandlingType.CLAIM, SampleLocations.HELSINKI);
            Assert.AreEqual(SampleLocations.HELSINKI, e1.Location);
        }

        [Test]
        public void TestCurrentLocationLoadEvent()
        {

            var ev = new HandlingEvent(cargo, new DateTime(), new DateTime(), HandlingType.LOAD,
                                                 SampleLocations.CHICAGO, SampleVoyages.CM004);

            Assert.AreEqual(SampleLocations.CHICAGO, ev.Location);
        }

        [Test]
        public void TestCurrentLocationUnloadEvent()
        {
            var ev = new HandlingEvent(cargo, new DateTime(), new DateTime(), HandlingType.UNLOAD,
                                                 SampleLocations.HAMBURG, SampleVoyages.CM004);

            Assert.AreEqual(SampleLocations.HAMBURG, ev.Location);
        }

        [Test]
        public void TestCurrentLocationReceivedEvent()
        {
            var ev = new HandlingEvent(cargo, new DateTime(), new DateTime(), HandlingType.RECEIVE,
                                                 SampleLocations.CHICAGO);

            Assert.AreEqual(SampleLocations.CHICAGO, ev.Location);
        }

        [Test]
        public void TestCurrentLocationClaimedEvent()
        {
            var ev = new HandlingEvent(cargo, new DateTime(), new DateTime(), HandlingType.CLAIM,
                                                 SampleLocations.CHICAGO);

            Assert.AreEqual(SampleLocations.CHICAGO, ev.Location);
        }

        [Test]
        public void TestParseType()
        {
            Assert.AreEqual(HandlingType.CLAIM, Enumeration.FromValue<HandlingType>("CLAIM"));
            Assert.AreEqual(HandlingType.LOAD, Enumeration.FromValue<HandlingType>("LOAD"));
            Assert.AreEqual(HandlingType.UNLOAD, Enumeration.FromValue<HandlingType>("UNLOAD"));
            Assert.AreEqual(HandlingType.RECEIVE, Enumeration.FromValue<HandlingType>("RECEIVE"));
            
        }

        [Test]
        [ExpectedException(typeof(ApplicationException), UserMessage = "Should not accept null constructor arguments")]
        public void TestParseTypeIllegal()
        {
            Enumeration.FromValue<HandlingType>("NOT_A_HANDLING_EVENT_TYPE");
        }

        [Test]
        public void TestEqualsAndSameAs()
        {
            var timeOccured = new DateTime();
            var timeRegistered = new DateTime();

            var ev1 = new HandlingEvent(cargo, timeOccured, timeRegistered, HandlingType.LOAD, SampleLocations.CHICAGO,
                                                  SampleVoyages.CM005);
            var ev2 = new HandlingEvent(cargo, timeOccured, timeRegistered, HandlingType.LOAD, SampleLocations.CHICAGO,
                                                  SampleVoyages.CM005);

            // Two handling events are not equal() even if all non-uuid fields are identical
            Assert.IsTrue(ev1.Equals(ev2));
            Assert.IsTrue(ev2.Equals(ev1));

            Assert.IsTrue(ev1.Equals(ev1));

            Assert.IsFalse(ev2.Equals(null));
            Assert.IsFalse(ev2.Equals(new Object()));
        }

        //TODO: egorgan revise commented tests
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void TestNewWithCarrierMovement()
        {
            var e1 = new HandlingEvent(cargo, new DateTime(), new DateTime(), HandlingType.LOAD, SampleLocations.HONGKONG, SampleVoyages.CM003);
            Assert.AreEqual(SampleLocations.HONGKONG, e1.Location);

            var e2 = new HandlingEvent(cargo, new DateTime(), new DateTime(), HandlingType.UNLOAD, SampleLocations.NEWYORK, SampleVoyages.CM003);
            Assert.AreEqual(SampleLocations.NEWYORK, e2.Location);

            // These event types prohibit a carrier movement association
            foreach (var type in new List<HandlingType>{HandlingType.CLAIM,HandlingType.RECEIVE,HandlingType.CUSTOMS})
            {
                try
                {
                    new HandlingEvent(cargo, new DateTime(), new DateTime(), type, SampleLocations.HONGKONG, SampleVoyages.CM003);
                }
                catch (Exception)
                {
                    Assert.Fail("Handling event type " + type + " prohibits carrier movement");                    
                }                                
            }

            // These event types requires a carrier movement association
            foreach (var type in new List<HandlingType> { HandlingType.LOAD, HandlingType.UNLOAD })
            {
                try
                {
                    new HandlingEvent(cargo, new DateTime(), new DateTime(), type, SampleLocations.HONGKONG, null);
                }
                catch (Exception)
                {
                    Assert.Fail("Handling event type " + type + " requires carrier movement");
                }
            }
        }
    }
}