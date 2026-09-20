namespace Infra.Cache;

public class CacheSettings
{
    public bool Enabled { get; set; }
    public string Provider { get; set; } = string.Empty;
    public int TimeCacheRolesHours { get; set; }
    public int TimeCacheRolesUserMinutes { get; set; }
}
