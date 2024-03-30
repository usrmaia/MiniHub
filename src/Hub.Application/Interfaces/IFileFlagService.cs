using Hub.Domain.DTOs;
using Hub.Domain.Entities;

namespace Hub.Application.Interfaces;

public interface IFileFlagService
{
    Task<FileFlag> AddFlag(string fileId, string flagId, UserDTO user);
    Task<FileFlag> RemoveFlag(string fileId, string flagId, UserDTO user);
}
