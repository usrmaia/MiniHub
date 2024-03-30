using Hub.Domain.DTOs;

namespace Hub.Application.Interfaces;

public interface IUserRoleService
{
    Task<UserDTO> AddToRole(string userId, string roleName);
    Task<UserDTO> RemoveFromRole(string userId, string roleName);
}
