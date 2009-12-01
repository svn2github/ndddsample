namespace NDDDSample.Interfaces.HandlingService.Messaging
{
    #region Usings

    using Application;
    using Domain.Model.Cargos;
    using Domain.Model.Handlings;
    using Infrastructure.Log;
    using Infrastructure.Messaging;

    #endregion

    public class EsbApplicationEventsImpl : IApplicationEvents
    {
        private readonly IMessageBus MessageBus;
        private readonly ILog logger = LogFactory.GetExternalServiceLogger();

        public EsbApplicationEventsImpl(IMessageBus messageBus)
        {
            MessageBus = messageBus;
        }

        public void CargoWasHandled(HandlingEvent evnt)
        {
            Cargo cargo = evnt.Cargo;
            logger.Info("Cargo was handled " + cargo);
            MessageBus.Publish(new CargoHandledMessage() {TrackingId = cargo.TrackingId.IdString});
        }

        public void CargoWasMisdirected(Cargo cargo)
        {
            logger.Info("Cargo was misdirected " + cargo);
            MessageBus.Publish(new CargoHandledMessageLogger() { TrackingId = cargo.TrackingId.IdString });
        }

        public void CargoHasArrived(Cargo cargo)
        {
            logger.Info("Cargo has arrived " + cargo);
            MessageBus.Publish(new CargoHandledMessageLogger() { TrackingId = cargo.TrackingId.IdString });
        }

        public void ReceivedHandlingEventRegistrationAttempt(HandlingEventRegistrationAttempt attempt)
        {
            logger.Info("Received handling event registration attempt " + attempt);
            MessageBus.Publish(new HandlingEventRegistrationAttemptMessage() {HandlingEventRegistration = attempt });
        }
    }
}