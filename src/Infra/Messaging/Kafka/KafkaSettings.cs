namespace Infra.Messaging.Kafka;

public class KafkaSettings
{
    public string BootstrapServers { get; set; } = string.Empty;
    public Dictionary<string, string> Topics { get; set; } = new();
    public int MessageRetry { get; set; }
    public int MessageTimeoutMs { get; set; }
    public int SocketTimeoutMs { get; set; }
    public bool EnableKafkaSend {  get; set; }
}
