using Application.Contracts.Events;
using Infra.Messaging.Kafka.Interfaces;
using Microsoft.Extensions.Options;

namespace Infra.Messaging.Kafka;

public class EventTopicMapper: IEventTopicMapper
{
    private readonly KafkaSettings _settings;

    public EventTopicMapper(IOptions<KafkaSettings> options)
    {
        _settings = options.Value;
    }

    public string GetTopic<T>()
    {
        return typeof(T) switch
        {
            var t when t == typeof(TravelStartedEvent)
                => _settings.Topics["TravelStarted"],

            var t when t == typeof(TravelEndedEvent)
                => _settings.Topics["TravelEnded"],

            var t when t == typeof(TravelReportEvent)
                => _settings.Topics["ReportTravels"],

            _ => throw new InvalidOperationException($"No topic mapping for {typeof(T).Name}")
        };
    }
}
