using Microsoft.AspNetCore.Identity;

namespace Hub.Domain.Entities;

public class Flag
{
    public string? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UserId { get; set; } = string.Empty;
    public IdentityUser? User { get; set; }
}
