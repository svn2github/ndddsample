namespace NDDDSample.Interfaces.HandlingService.Messaging
{
    #region Usings

    using System;
    using Application;
    using Infrastructure.Log;
    using Infrastructure.Messaging;

    #endregion

    public class HandlingEventRegistrationAttemptHandler : IMessageHandler<HandlingEventRegistrationAttemptMessage>
    {
        private IHandlingEventService handlingEventService;
        private static ILog logger = LogFactory.GetExternalServiceLogger();

        public HandlingEventRegistrationAttemptHandler(IHandlingEventService handlingEventService)
        {
            this.handlingEventService = handlingEventService;
        }


        public void Handle(HandlingEventRegistrationAttemptMessage message)
        {
            try
            {
                //TODO: Revise transaciton and UoW logic
                using (Rhino.Commons.UnitOfWork.Start())
                {
                    var attempt = message.HandlingEventRegistration;
                    handlingEventService.RegisterHandlingEvent(
                        attempt.CompletionTime,
                        attempt.TrackingId,
                        attempt.VoyageNumber,
                        attempt.UnLocode,
                        attempt.Type);

                    Rhino.Commons.UnitOfWork.Current.Flush();
                }
            }
            catch (Exception e)
            {
                logger.Error(e, e);
            }
        }
    }
}