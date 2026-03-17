namespace FeatureFolio.Application.Entries;

public class ImagesUploadedEntry : BaseEntry
{
    public ICollection<string> ImagesUrls { get; set; } = new List<string>();
}
