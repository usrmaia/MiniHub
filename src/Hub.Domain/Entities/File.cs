using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace Hub.Domain.Entities;

public class FileE
{
    public string? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Length { get; set; }
    public string Path { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string? DirectoryId { get; set; }
    [JsonIgnore]
    public DirectoryE? Directory { get; set; }
    public List<Flag> Flags { get; set; } = [];
    public List<IdentityRole> Roles { get; set; } = [];
    public string UserId { get; set; } = string.Empty;
    public IdentityUser? User { get; set; }
}