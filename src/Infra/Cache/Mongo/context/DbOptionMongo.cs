namespace Infra.Cache.Mongo.context;

public class DbOptionMongo
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public int ServerSelectionTimeoutSeconds { get; set; }
}
