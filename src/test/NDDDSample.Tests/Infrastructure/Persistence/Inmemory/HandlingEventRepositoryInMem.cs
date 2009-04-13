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

        #region IHandlingEventRepository Members

        public void Store(HandlingEvent evnt)
        {
            TrackingId trackingId = evnt.Cargo.TrackingId;

            List<HandlingEvent> list;
            if (!eventMap.ContainsKey(trackingId))
            {
                list = new List<HandlingEvent>();
                eventMap.Add(trackingId, list);
            }

            list = eventMap[trackingId];

            list.Add(evnt);
        }


        public HandlingHistory LookupHandlingHistoryOfCargo(TrackingId trackingId)
        {
            List<HandlingEvent> events;

            if (!eventMap.ContainsKey(trackingId))
            {
                events = new List<HandlingEvent>();
            }
            else
            {
                events = eventMap[trackingId];
            }

            return new HandlingHistory(events);
        }

        #endregion
    }
}