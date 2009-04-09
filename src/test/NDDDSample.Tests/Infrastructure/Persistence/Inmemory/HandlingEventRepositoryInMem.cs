namespace NDDDSample.Tests.Infrastructure.Persistence.Inmemory
{
    #region Usings

    using System.Collections.Generic;
    using NDDDSample.Domain.Model.Cargos;
    using NDDDSample.Domain.Model.Handlings;

    #endregion

    public class HandlingEventRepositoryInMem : IHandlingEventRepository
    {
        private readonly IDictionary<TrackingId, List<HandlingEvent>> eventMap =
            new Dictionary<TrackingId, List<HandlingEvent>>();

        public void Store(HandlingEvent evnt)
        {
            TrackingId trackingId = evnt.Cargo.TrackingId;
            List<HandlingEvent> list = eventMap[trackingId];
            if (list == null)
            {
                list = new List<HandlingEvent>();
                eventMap.Add(trackingId, list);
            }
            list.Add(evnt);
        }


        public HandlingHistory LookupHandlingHistoryOfCargo(TrackingId trackingId)
        {
            List<HandlingEvent> events = eventMap[trackingId];
            if (events == null)
            {
                events = new List<HandlingEvent>();
            }

            return new HandlingHistory(events);
        }
    }
}