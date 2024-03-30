using Hub.Application.Interfaces;
using Hub.Domain.DTOs;
using Hub.Domain.Entities;
using Hub.Domain.Exceptions;
using Hub.Domain.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace Hub.Application.Services;

public class DirectoryService : IDirectoryService
{
    private readonly IDirectoryRepository _directoryRepository;

    public DirectoryService(IDirectoryRepository directoryRepository) =>
        _directoryRepository = directoryRepository;

    public async Task<List<DirectoryE>> GetAll() =>
        await _directoryRepository.GetAll();

    public async Task<DirectoryE> GetById(string id)
    {
        if (!await _directoryRepository.ExistsById(id))
            throw new AppException("Diretório não encontrado", HttpStatusCode.NotFound);

        return await _directoryRepository.GetById(id);
    }

    public Task<int> GetCount() =>
        _directoryRepository.GetCount();

    public async Task<DirectoryE> Create(DirectoryE directory)
    {
        if (!directory.ParentId.IsNullOrEmpty() && !await _directoryRepository.ExistsById(directory.ParentId))
            throw new AppException("Diretório pai não encontrado", HttpStatusCode.NotFound);

        if (await _directoryRepository.ExistsByNameAndParentId(directory.Name, directory.ParentId))
            throw new AppException("Diretório já existe", HttpStatusCode.Conflict);

        var parents = new List<string>();
        if (!directory.ParentId.IsNullOrEmpty())
            parents = await _directoryRepository.GetParentsNameById(directory.ParentId!);

        var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "hub", Path.Combine(parents.ToArray()), directory.Name);

        if (Directory.Exists(directoryPath))
            throw new AppException("Diretório já existe", HttpStatusCode.Conflict);

        Directory.CreateDirectory(directoryPath);

        return await _directoryRepository.Add(directory);
    }

    public async Task<DirectoryE> Update(DirectoryE newDirectory)
    {
        if (!await _directoryRepository.ExistsById(newDirectory.Id))
            throw new AppException("Diretório não encontrado", HttpStatusCode.NotFound);

        var oldDirectory = await _directoryRepository.GetById(newDirectory.Id!);

        if (oldDirectory.UserId != newDirectory.UserId)
            throw new AppException("Usuário não possui permissão para editar este diretório", HttpStatusCode.Forbidden);

        if (oldDirectory.ParentId != newDirectory.ParentId && !await _directoryRepository.ExistsById(newDirectory.ParentId))
            throw new AppException("Diretório pai não encontrado", HttpStatusCode.NotFound);

        if (oldDirectory.Name != newDirectory.Name)
            throw new AppException("Não é possível alterar o nome do diretório", HttpStatusCode.BadRequest);

        oldDirectory.ParentId = newDirectory.ParentId;

        return await _directoryRepository.Update(oldDirectory);
    }

    public async Task<DirectoryE> Delete(string id, UserDTO user)
    {
        if (!await _directoryRepository.ExistsById(id))
            throw new AppException("Diretório não encontrado", HttpStatusCode.NotFound);

        var directory = await _directoryRepository.GetById(id);

        if (directory.UserId != user.Id)
            throw new AppException("Usuário não possui permissão para deletar este diretório", HttpStatusCode.Forbidden);

        return await _directoryRepository.Remove(directory);
    }
}
