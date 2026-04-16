namespace Application.Interfaces.Messaging;

public interface IMessageProducer
{
    Task PublishAsync<T>(T message);
}
