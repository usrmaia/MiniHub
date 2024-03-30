using Microsoft.AspNetCore.Identity;

namespace Hub.Domain.Entities;

public class FileRole
{
    public string FileId { get; set; } = string.Empty;
    public FileE? File { get; set; }
    public string RoleId { get; set; } = string.Empty;
    public IdentityRole? Role { get; set; }
}
