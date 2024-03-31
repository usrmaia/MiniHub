using Microsoft.AspNetCore.Identity;

namespace Hub.Domain.Entities;

public class DirectoryE
{
    public string? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ParentId { get; set; }
    public DirectoryE? Parent { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public List<FileE> Files { get; set; } = [];

    public List<Flag> Flags { get; set; } = [];

    public List<IdentityRole> Roles { get; set; } = [];

    public string UserId { get; set; } = string.Empty;
    public IdentityUser? User { get; set; }
}
