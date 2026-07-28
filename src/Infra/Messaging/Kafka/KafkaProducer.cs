using Application.Contracts.Abstractions;
using Confluent.Kafka;
using Infra.Messaging.Kafka.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using System.Text.Json;
namespace Infra.Messaging.Kafka;

public class KafkaProducer : IMessageProducer
{
    private readonly IProducer<string, string> _producer;
    private readonly IEventTopicMapper _mapper;
    private readonly AsyncRetryPolicy _retryPolicy;
    private readonly ILogger<KafkaProducer> _logger;

    public KafkaProducer(IOptions<KafkaSettings> options, IEventTopicMapper mapper, ILogger<KafkaProducer> logger)
    {
        _mapper = mapper;
        _logger = logger;
        var settings = options.Value;
        
        if (settings.EnableKafkaSend)
        {
            var configProducer = new ProducerConfig
            {
                BootstrapServers = settings.BootstrapServers,
                MessageTimeoutMs = settings.MessageTimeoutMs,
                SocketTimeoutMs = settings.SocketTimeoutMs
            };
            _producer = new ProducerBuilder<string, string>(configProducer).Build();
            _retryPolicy = Policy
                .Handle<ProduceException<string, string>>()
                .WaitAndRetryAsync(
                    settings.MessageRetry,
                    retryAttempt =>
                        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        _logger.LogWarning(
                            exception,
                            "Erro ao publicar no Kafka. Retry {RetryCount}",
                            retryCount);
                    });
        }
    }

    public async Task PublishAsync<T>(T message, CancellationToken cancellationToken)
    {
        if (_producer is null)
        {
            _logger.LogInformation("O kafka está inabilitado no momento");
            return;
        }

        var topic = _mapper.GetTopic<T>();

        var json = JsonSerializer.Serialize(message);
        try
        {
            await _retryPolicy.ExecuteAsync(async () =>
            {
                await _producer.ProduceAsync(topic, new Message<string, string>
                {
                    Key = Guid.NewGuid().ToString(),
                    Value = json
                }, cancellationToken);
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Erro ao publicar mensagem no Kafka");

            throw;
        }
    }
}
