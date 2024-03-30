using Microsoft.AspNetCore.Identity;

namespace Hub.Domain.Repositories;

public interface IUserRoleRepository : IBaseRepository<IdentityUserRole<string>>
{
    Task<bool> ExistsByUserAndRoleName(IdentityUser user, string roleName);
    Task<bool> ExistsByUserIdAndRoleName(string userId, string roleName);
    Task<bool> Add(IdentityUser user, string roleName);
    Task<bool> Remove(IdentityUser user, string roleName);
}
