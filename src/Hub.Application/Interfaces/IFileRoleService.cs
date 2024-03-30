using Hub.Domain.DTOs;
using Hub.Domain.Entities;

namespace Hub.Application.Interfaces;

public interface IFileRoleService
{
    Task<FileRole> AddRole(string fileId, string roleId, UserDTO user);
    Task<FileRole> RemoveRole(string fileId, string roleId, UserDTO user);
}
