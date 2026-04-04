namespace FeatureFolio.Application.Entries;

public class ImagesUploadedEntry : BaseEntry
{
    public ICollection<string> ImageNames { get; set; } = new List<string>();
    public string UserGuid { get; set; } = String.Empty;

    public string EventId{ get; set; } = String.Empty;
}
