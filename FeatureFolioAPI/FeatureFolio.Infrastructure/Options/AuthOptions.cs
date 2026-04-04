namespace FeatureFolio.Infrastructure.Options;

public class AuthOptions
{
    public const string SectionName = "Auth";
    public GoogleOptions GoogleOptions { get; set; } = new();
}

public class GoogleOptions
{
    public string ClientId { get; set; } = String.Empty;
}