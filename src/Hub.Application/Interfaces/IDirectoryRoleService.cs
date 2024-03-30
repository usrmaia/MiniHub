using Hub.Domain.DTOs;
using Hub.Domain.Entities;

namespace Hub.Application.Interfaces;

public interface IDirectoryRoleService
{
    Task<DirectoryRole> AddRole(string directoryId, string roleId, UserDTO user);
    Task<DirectoryRole> RemoveRole(string directoryId, string roleId, UserDTO user);
}
