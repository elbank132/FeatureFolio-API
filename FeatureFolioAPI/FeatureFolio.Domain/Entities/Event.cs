namespace FeatureFolio.Domain.Entities;

public class Event
{
    public Guid EventId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string EventName { get; set; } = string.Empty;
    public DateTime Date { get; set; }

    public User? User { get; set; }
    public ICollection<Cluster> Clusters { get; set; } = [];
}
