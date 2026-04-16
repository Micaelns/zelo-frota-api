using Application.Interfaces.Messaging;
using Confluent.Kafka;
using Infra.Messaging.Kafka.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.Json;
namespace Infra.Messaging.Kafka;

public class KafkaProducer : IMessageProducer
{
    private readonly IProducer<string, string> _producer;
    private readonly IEventTopicMapper _mapper;

    public KafkaProducer(IOptions<KafkaSettings> options, IEventTopicMapper mapper)
    {
        _mapper = mapper;
        var settings = options.Value;
        var configProducer = new ProducerConfig
        {
            BootstrapServers = settings.BootstrapServers
        };

        _producer = new ProducerBuilder<string, string>(configProducer).Build();
    }

    public async Task PublishAsync<T>(T message)
    {
        var topic = _mapper.GetTopic<T>();

        var json = JsonSerializer.Serialize(message);

        await _producer.ProduceAsync(topic, new Message<string, string>
        {
            Key = Guid.NewGuid().ToString(),
            Value = json
        });
    }
}
