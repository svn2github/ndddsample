namespace NDDDSample.Infrastructure.Messaging
{
    /// <summary>
    /// Later the existing michanism can be replaced with an 'real' ESB
    /// such as NServiceBus or Masstransit
    /// </summary>
    public interface IMessageBus
    {
        void Publish<TMessage>(TMessage message) where TMessage : class, IMessage;
    }
}