namespace FeatureFolio.Infrastructure.Options;

public class JwtOptions
{
    public const string SectionName = "JwtSettings";

    public string Issuer { get; set; } = String.Empty;
    public string Audience { get; set; } = String.Empty;
    public int ExpirationInMinutes { get; set; }
}
