namespace Application.Contracts.Abstractions;

public interface IMessageProducer
{
    Task PublishAsync<T>(T message, CancellationToken cancellationToken);
}
