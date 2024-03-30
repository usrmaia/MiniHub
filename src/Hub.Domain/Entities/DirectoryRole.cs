using Microsoft.AspNetCore.Identity;

namespace Hub.Domain.Entities;

public class DirectoryRole
{
    public string DirectoryId { get; set; } = string.Empty;
    public DirectoryE? Directory { get; set; }
    public string RoleId { get; set; } = string.Empty;
    public IdentityRole? Role { get; set; }
}
