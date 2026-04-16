namespace Application.Contracts.Messaging;

public interface IMessageProducer
{
    Task PublishAsync<T>(T message);
}
