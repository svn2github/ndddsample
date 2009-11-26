namespace NDDDSample.Infrastructure.Messaging
{
    using Castle.Windsor;

    public class MessageBus : IMessageBus
    {
        private readonly IWindsorContainer container;

        public MessageBus(IWindsorContainer container)
        {
            this.container = container;
        }

        public void Publish<TMessage>(TMessage message) where TMessage : class, IMessage
        {
            var eventHandlers = container.ResolveAll<IMessageHandler<TMessage>>();
            foreach (var eventHandler in eventHandlers)
            {
                eventHandler.Handle(message);
            }
        }
    }
}