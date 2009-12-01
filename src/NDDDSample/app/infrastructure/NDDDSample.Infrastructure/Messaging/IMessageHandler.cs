namespace NDDDSample.Infrastructure.Messaging
{
    public interface IMessageHandler<T>
        where T: IMessage
    {
        void Handle(T message);
    }
}