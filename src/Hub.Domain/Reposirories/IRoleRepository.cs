using Hub.Domain.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Hub.Domain.Repositories;

public interface IRoleRepository : IBaseRepository<IdentityRole>
{
    Task<List<RoleDTO>> Query();
    bool IsDefault(string name);
    Task<bool> ExistsById(string id);
    Task<bool> ExistsByName(string name);
    Task<IdentityRole> GetById(string id);
    Task<IdentityRole> GetByName(string name);
    new Task<IdentityRole> Add(IdentityRole role);
    new Task<IdentityRole> Update(IdentityRole role);
    Task<IdentityRole> Remove(string name);
}
