using System.Net;
using Hub.Application.Interfaces;
using Hub.Domain.DTOs;
using Hub.Domain.Entities;
using Hub.Domain.Exceptions;
using Hub.Domain.Repositories;

namespace Hub.Application.Services;

public class FileRoleService : IFileRoleService
{
    private readonly IFileRepository _fileRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IFileRoleRepository _fileRoleRepository;

    public FileRoleService(IFileRepository fileRepository,
        IRoleRepository roleRepository,
        IFileRoleRepository fileRoleRepository)
    {
        _fileRepository = fileRepository;
        _roleRepository = roleRepository;
        _fileRoleRepository = fileRoleRepository;
    }

    public async Task<FileRole> AddRole(string fileId, string roleId, UserDTO user)
    {
        if (!await _fileRepository.ExistsById(fileId))
            throw new AppException("Arquivo não encontrado", HttpStatusCode.NotFound);

        var file = await _fileRepository.GetById(fileId);

        if (file.UserId != user.Id)
            throw new AppException("Usuário não autorizado", HttpStatusCode.Unauthorized);

        if (!await _roleRepository.ExistsById(roleId))
            throw new AppException("Role não encontrada", HttpStatusCode.NotFound);

        if (await _fileRoleRepository.ExistsByFileAndRole(fileId, roleId))
            throw new AppException("Role já adicionada ao arquivo", HttpStatusCode.Conflict);

        return await _fileRoleRepository.Add(new FileRole { FileId = fileId, RoleId = roleId });
    }

    public async Task<FileRole> RemoveRole(string fileId, string roleId, UserDTO user)
    {
        if (!await _fileRepository.ExistsById(fileId))
            throw new AppException("Arquivo não encontrado", HttpStatusCode.NotFound);

        var file = await _fileRepository.GetById(fileId);

        if (file.UserId != user.Id)
            throw new AppException("Usuário não autorizado", HttpStatusCode.Unauthorized);

        if (!await _roleRepository.ExistsById(roleId))
            throw new AppException("Role não encontrada", HttpStatusCode.NotFound);

        if (!await _fileRoleRepository.ExistsByFileAndRole(fileId, roleId))
            throw new AppException("Role não encontrada no arquivo", HttpStatusCode.Conflict);

        return await _fileRoleRepository.Remove(new FileRole { FileId = fileId, RoleId = roleId });
    }
}
