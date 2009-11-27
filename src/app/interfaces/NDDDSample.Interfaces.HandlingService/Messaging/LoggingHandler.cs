namespace NDDDSample.Interfaces.HandlingService.Messaging
{
    #region Usings

    using Infrastructure.Log;
    using Infrastructure.Messaging;

    #endregion

    public class LoggingHandler : IMessageHandler<CargoHandledMessageLogger>
    {
        private ILog logger = LogFactory.GetExternalServiceLogger();

        public void Handle(CargoHandledMessageLogger message)
        {
            logger.Debug("Received message: " + message);
        }
    }
}