namespace NDDDSample.Interfaces.HandlingService.Messaging
{
    #region Usings

    using Application;
    using Infrastructure.Messaging;

    #endregion

    public class HandlingEventRegistrationAttemptMessage : IMessage
    {
        public HandlingEventRegistrationAttempt HandlingEventRegistration { get; set; }
    }
}