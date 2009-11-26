namespace NDDDSample.Interfaces.HandlingService.Messaging
{
    #region Usings

    using Infrastructure.Messaging;

    #endregion

    public class HandlingEventRegistrationAttemptMessage : IMessage
    {
        public HandlingEventRegistrationAttempt HandlingEventRegistration { get; set; }
    }
}