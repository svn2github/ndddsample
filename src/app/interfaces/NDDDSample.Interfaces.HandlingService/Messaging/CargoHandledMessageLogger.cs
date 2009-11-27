namespace NDDDSample.Interfaces.HandlingService.Messaging
{
    #region Usings

    using Infrastructure.Messaging;

    #endregion

    public class CargoHandledMessageLogger : IMessage
    {
        public string TrackingId { get; set; }
    }
}