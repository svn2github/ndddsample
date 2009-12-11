namespace NDDDSample.Tests.Domain.Model.Cargos
{
    #region Usings

    using System;
    using NDDDSample.Domain.Model.Cargos;
    using NDDDSample.Domain.Model.Locations;
    using NUnit.Framework;

    #endregion

    [TestFixture, Category(UnitTestCategories.DomainModel)]
    public class DeliveryTest
    {
        private Cargo cargo = new Cargo(new TrackingId("XYZ"),
                                        new RouteSpecification(SampleLocations.HONGKONG, SampleLocations.NEWYORK,
                                                               DateTime.Now));

        [Test]
        public void TestToSilenceWarnings()
        {
            //TODO: atrosin revise the test
            Assert.IsTrue(true);
        }


        //TODO: atrosin revise commented tests

   /*public void testEvensOrderedByTimeOccured() 
     {
      DateFormat df = new SimpleDateFormat("yyyy-MM-dd");
      HandlingEvent he1 = new HandlingEvent(cargo, df.parse("2010-01-03"), dateTime, HandlingEvent.Type.RECEIVE, NEWYORK);
      HandlingEvent he2 = new HandlingEvent(cargo, df.parse("2010-01-01"), dateTime, HandlingEvent.Type.LOAD, NEWYORK, CM003);
      HandlingEvent he3 = new HandlingEvent(cargo, df.parse("2010-01-04"), dateTime, HandlingEvent.Type.CLAIM, HONGKONG);
      HandlingEvent he4 = new HandlingEvent(cargo, df.parse("2010-01-02"), dateTime, HandlingEvent.Type.UNLOAD, HONGKONG, CM004);
      Delivery dh = new Delivery(Arrays.asList(he1, he2, he3, he4));

      List<HandlingEvent> orderEvents = dh.history();
      Assert.AreEqual(4, orderEvents.size());
      assertSame(he2, orderEvents.get(0));
      assertSame(he4, orderEvents.get(1));
      assertSame(he1, orderEvents.get(2));
      assertSame(he3, orderEvents.get(3));
    }

    public void testCargoStatusFromLastHandlingEvent() 
    {
      Set<HandlingEvent> events = new HashSet<HandlingEvent>();
      Delivery delivery = new Delivery(events);

      Assert.AreEqual(TransportStatus.NOT_RECEIVED, delivery.transportStatus());

      events.add(new HandlingEvent(cargo, new Date(10), new Date(11), HandlingEvent.Type.RECEIVE, HAMBURG));
      delivery = new Delivery(events);
      Assert.AreEqual(TransportStatus.IN_PORT, delivery.transportStatus());

      events.add(new HandlingEvent(cargo, new Date(20), new Date(21), HandlingEvent.Type.LOAD, HAMBURG, CM005));
      delivery = new Delivery(events);
      Assert.AreEqual(TransportStatus.ONBOARD_CARRIER, delivery.transportStatus());

      events.add(new HandlingEvent(cargo, new Date(30), new Date(31), HandlingEvent.Type.UNLOAD, HAMBURG, CM006));
      delivery = new Delivery(events);
      Assert.AreEqual(TransportStatus.IN_PORT, delivery.transportStatus());

      events.add(new HandlingEvent(cargo, new Date(40), new Date(41), HandlingEvent.Type.CLAIM, HAMBURG));
      delivery = new Delivery(events);
      Assert.AreEqual(TransportStatus.CLAIMED, delivery.transportStatus());
    }

    public void testLastKnownLocation()
    {
      Set<HandlingEvent> events = new HashSet<HandlingEvent>();
      Delivery delivery = new Delivery(events);

      Assert.AreEqual(Location.UNKNOWN, delivery.lastKnownLocation());

      events.add(new HandlingEvent(cargo, new Date(10), new Date(11), HandlingEvent.Type.RECEIVE, HAMBURG));
      delivery = new Delivery(events);

      Assert.AreEqual(HAMBURG, delivery.lastKnownLocation());

      events.add(new HandlingEvent(cargo, new Date(20), new Date(21), HandlingEvent.Type.LOAD, HAMBURG, CM003));
      delivery = new Delivery(events);

      Assert.AreEqual(HAMBURG, delivery.lastKnownLocation());
    }    */
    }
}