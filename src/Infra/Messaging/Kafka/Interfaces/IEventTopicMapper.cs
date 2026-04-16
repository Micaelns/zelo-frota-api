namespace Infra.Messaging.Kafka.Interfaces;

public interface IEventTopicMapper
{
    public string GetTopic<T>();
}
