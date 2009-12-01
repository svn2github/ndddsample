namespace NDDDSample.Interfaces.HandlingService.Messaging
{
    #region Usings

    using Infrastructure.Messaging;

    #endregion

    /// <summary>
    /// Represents a Message with a tracking Id as a payload.
    /// </summary>
    public class CargoHandledMessage : IMessage
    {
        public string TrackingId { get; set; }
    }
}