using Hub.Domain.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Hub.Application.Interfaces;

public interface IRoleService
{
    Task<List<RoleDTO>> Query();
    Task<RoleDTO> GetById(int id);
    Task<IdentityRole> Create(IdentityRole role);
    Task<IdentityRole> Update(IdentityRole role);
    Task<bool> Delete(string id);
}
