using FeatureFolio.Domain.Enums;

namespace FeatureFolio.Domain.Entities;

public class User
{
    public string UserId { get; set; } = string.Empty;
    public Role Role { get; set; } = Role.User;
    public ICollection<Event> Events { get; set; } = [];
}
