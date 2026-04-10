namespace FeatureFolio.Domain.Entities;

public class Cluster
{
    public Guid ClusterId { get; set; }
    public Guid EventId { get; set; }
    public List<string> ImageBlobNames { get; set; } = new List<string>();

    public Event? Event { get; set; }
}
