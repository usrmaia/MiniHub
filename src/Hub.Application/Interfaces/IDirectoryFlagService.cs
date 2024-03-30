using Hub.Domain.DTOs;
using Hub.Domain.Entities;

namespace Hub.Application.Interfaces;

public interface IDirectoryFlagService
{
    Task<DirectoryFlag> AddFlag(string directoryId, string flagId, UserDTO user);
    Task<DirectoryFlag> RemoveFlag(string directoryId, string flagId, UserDTO user);
}
