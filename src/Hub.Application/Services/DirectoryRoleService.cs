using Hub.Application.Interfaces;
using Hub.Domain.DTOs;
using Hub.Domain.Entities;
using Hub.Domain.Exceptions;
using Hub.Domain.Repositories;
using System.Net;

namespace Hub.Application.Services;

public class DirectoryRoleService : IDirectoryRoleService
{
    private readonly IDirectoryRepository _directoryRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IDirectoryRoleRepository _directoryRoleRepository;

    public DirectoryRoleService(
        IDirectoryRepository directoryRepository,
        IRoleRepository roleRepository,
        IDirectoryRoleRepository directoryRoleRepository)
    {
        _directoryRepository = directoryRepository;
        _roleRepository = roleRepository;
        _directoryRoleRepository = directoryRoleRepository;
    }

    public async Task<DirectoryRole> AddRole(string directoryId, string roleId, UserDTO user)
    {
        if (!await _directoryRepository.ExistsById(directoryId))
            throw new AppException("Diretório não encontrado", HttpStatusCode.NotFound);

        var directory = await _directoryRepository.GetById(directoryId);

        if (directory.UserId != user.Id)
            throw new AppException("Diretório não pertence ao usuário", HttpStatusCode.Forbidden);

        if (!await _roleRepository.ExistsById(roleId))
            throw new AppException("Perfil não encontrado", HttpStatusCode.NotFound);

        if (await _directoryRoleRepository.ExistsByDirectoryAndRole(directoryId, roleId))
            throw new AppException("Perfil já adicionado", HttpStatusCode.Conflict);

        return await _directoryRoleRepository.Add(new DirectoryRole { DirectoryId = directoryId, RoleId = roleId });
    }

    public async Task<DirectoryRole> RemoveRole(string directoryId, string roleId, UserDTO user)
    {
        if (!await _directoryRepository.ExistsById(directoryId))
            throw new AppException("Diretório não encontrado", HttpStatusCode.NotFound);

        var directory = await _directoryRepository.GetById(directoryId);

        if (directory.UserId != user.Id)
            throw new AppException("Diretório não pertence ao usuário", HttpStatusCode.Forbidden);

        if (!await _roleRepository.ExistsById(roleId))
            throw new AppException("Perfil não encontrado", HttpStatusCode.NotFound);

        if (!await _directoryRoleRepository.ExistsByDirectoryAndRole(directoryId, roleId))
            throw new AppException("Diretório não possui este perfil", HttpStatusCode.Conflict);

        return await _directoryRoleRepository.Remove(new DirectoryRole { DirectoryId = directoryId, RoleId = roleId });
    }
}
