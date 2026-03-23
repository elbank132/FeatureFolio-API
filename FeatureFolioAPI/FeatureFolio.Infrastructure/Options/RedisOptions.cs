namespace FeatureFolio.Infrastructure.Options;

public class RedisOptions
{
    public const string SectionName = "Redis";
    public string KeyPrefix { get; set; } = String.Empty;
    public string Configuration { get; set; } = String.Empty;
    public int Port { get; set; }
    public int TTL { get; set; }
}
