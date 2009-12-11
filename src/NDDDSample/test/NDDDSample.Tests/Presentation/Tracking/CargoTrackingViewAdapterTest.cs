namespace NDDDSample.Tests.Presentation.Tracking
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using NDDDSample.Domain.Model.Cargos;
    using NDDDSample.Domain.Model.Handlings;
    using NDDDSample.Domain.Model.Locations;
    using NDDDSample.Domain.Model.Voyages;
    using NUnit.Framework;
    using Web.Controllers.Tracking;

    #endregion

    [TestFixture, Category(UnitTestCategories.Controllers)]
    public class CargoTrackingViewAdapterTest
    {
        [Test]
        public void CanCreateTest()
        {
            var cargo = new Cargo(new TrackingId("XYZ"),
                                    new RouteSpecification(SampleLocations.HANGZOU, SampleLocations.HELSINKI,
                                                           DateTime.Now));

            var events = new List<HandlingEvent>
                             {
                                 new HandlingEvent(cargo, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), HandlingType.RECEIVE,
                                                   SampleLocations.HANGZOU),
                                 new HandlingEvent(cargo, DateTime.Now.AddDays(3), DateTime.Now.AddDays(4), HandlingType.LOAD,
                                                   SampleLocations.HANGZOU, SampleVoyages.CM001),
                                 new HandlingEvent(cargo, DateTime.Now.AddDays(5), DateTime.Now.AddDays(6), HandlingType.UNLOAD,
                                                   SampleLocations.HELSINKI, SampleVoyages.CM001)
                             };

            cargo.DeriveDeliveryProgress(new HandlingHistory(events));

            var adapter = new CargoTrackingViewAdapter(cargo, events);

            Assert.AreEqual("XYZ", adapter.TrackingId);
            Assert.AreEqual("Hangzhou", adapter.Origin);
            Assert.AreEqual("Helsinki", adapter.Destination);
            Assert.AreEqual("In port Helsinki", adapter.GetStatusText());

            IEnumerator<HandlingEventViewAdapter> it = adapter.Events.GetEnumerator();

            it.MoveNext();
            HandlingEventViewAdapter evnt = it.Current;
            Assert.AreEqual("RECEIVE", evnt.Type);
            Assert.AreEqual("Hangzhou", evnt.Location);
            Assert.AreEqual(GetDateFormated(1), evnt.Time);
            Assert.AreEqual("", evnt.VoyageNumber);
            Assert.IsTrue(evnt.IsExpected);

            it.MoveNext();
            evnt = it.Current;
            Assert.AreEqual("LOAD", evnt.Type);
            Assert.AreEqual("Hangzhou", evnt.Location);
            Assert.AreEqual(GetDateFormated(3), evnt.Time);
            Assert.AreEqual("CM001", evnt.VoyageNumber);
            Assert.IsTrue(evnt.IsExpected);

            it.MoveNext();
            evnt = it.Current;
            Assert.AreEqual("UNLOAD", evnt.Type);
            Assert.AreEqual("Helsinki", evnt.Location);
            Assert.AreEqual(GetDateFormated(5), evnt.Time);
            Assert.AreEqual("CM001", evnt.VoyageNumber);
            Assert.IsTrue(evnt.IsExpected);
        }

        private static string GetDateFormated(double days)
        {
            return DateTime.Now.AddDays(days).ToString(HandlingEventViewAdapter.FORMAT);
        }
    }
}