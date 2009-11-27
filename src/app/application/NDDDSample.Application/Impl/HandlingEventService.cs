namespace NDDDSample.Application.Impl
{
    #region Usings

    using System;
    using System.Transactions;
    using Domain.Model.Cargos;
    using Domain.Model.Handlings;
    using Domain.Model.Locations;
    using Domain.Model.Voyages;
    using Infrastructure.Log;

    #endregion

    public class HandlingEventService : IHandlingEventService
    {
        private readonly IApplicationEvents applicationEvents;
        private readonly HandlingEventFactory handlingEventFactory;
        private readonly IHandlingEventRepository handlingEventRepository;
        private readonly ILog logger = LogFactory.GetApplicationLayerLogger();

        public HandlingEventService(IHandlingEventRepository handlingEventRepository,
                                    IApplicationEvents applicationEvents,
                                    HandlingEventFactory handlingEventFactory)
        {
            this.handlingEventRepository = handlingEventRepository;
            this.applicationEvents = applicationEvents;
            this.handlingEventFactory = handlingEventFactory;
        }

        #region IHandlingEventService Members

        public void RegisterHandlingEvent(DateTime completionTime,
                                          TrackingId trackingId,
                                          VoyageNumber voyageNumber,
                                          UnLocode unLocode,
                                          HandlingType type)
        {
            //TODO: Revise transaciton and UoW logic
            using (var transactionScope = new TransactionScope())
            {
                var registrationTime = new DateTime();

                /* Using a factory to create a HandlingEvent (aggregate). This is where
               it is determined wether the incoming data, the attempt, actually is capable
               of representing a real handling event. */
                HandlingEvent evnt = handlingEventFactory.CreateHandlingEvent(
                    registrationTime, completionTime, trackingId, voyageNumber, unLocode, type);

                /* Store the new handling event, which updates the persistent
                   state of the handling event aggregate (but not the cargo aggregate -
                   that happens asynchronously!)*/

                handlingEventRepository.Store(evnt);

                /* Publish an event stating that a cargo has been handled. */
                applicationEvents.CargoWasHandled(evnt);

                transactionScope.Complete();
            }
            logger.Info("Registered handling event");
        }

        #endregion
    }
}