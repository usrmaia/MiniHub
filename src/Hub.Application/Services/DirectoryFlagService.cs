using Hub.Application.Interfaces;
using Hub.Domain.DTOs;
using Hub.Domain.Entities;
using Hub.Domain.Exceptions;
using Hub.Domain.Repositories;
using System.Net;

namespace Hub.Application.Services;

public class DirectoryFlagService : IDirectoryFlagService
{
    private readonly IDirectoryFlagRepository _directoryFlagRepository;
    private readonly IDirectoryRepository _directoryRepository;
    private readonly IFlagRepository _flagRepository;

    public DirectoryFlagService(
        IDirectoryFlagRepository directoryFlagRepository,
        IDirectoryRepository directoryRepository,
        IFlagRepository flagRepository)
    {
        _directoryFlagRepository = directoryFlagRepository;
        _directoryRepository = directoryRepository;
        _flagRepository = flagRepository;
    }

    public async Task<DirectoryFlag> AddFlag(string directoryId, string flagId, UserDTO user)
    {
        if (!await _directoryRepository.ExistsById(directoryId))
            throw new AppException("Diretório não encontrado", HttpStatusCode.NotFound);

        var directory = await _directoryRepository.GetById(directoryId);

        if (directory.UserId != user.Id)
            throw new AppException("Diretório não pertence ao usuário", HttpStatusCode.Forbidden);

        if (!await _flagRepository.ExistsById(flagId))
            throw new AppException("Flag não encontrado", HttpStatusCode.NotFound);

        if (await _directoryFlagRepository.ExistsByDirectoryAndFlag(directoryId, flagId))
            throw new AppException("Flag já adicionada", HttpStatusCode.Conflict);

        return await _directoryFlagRepository.Add(new DirectoryFlag { DirectoryId = directoryId, FlagId = flagId });
    }

    public async Task<DirectoryFlag> RemoveFlag(string directoryId, string flagId, UserDTO user)
    {
        if (!await _directoryRepository.ExistsById(directoryId))
            throw new AppException("Diretório não encontrado", HttpStatusCode.NotFound);

        var directory = await _directoryRepository.GetById(directoryId);

        if (directory.UserId != user.Id)
            throw new AppException("Diretório não pertence ao usuário", HttpStatusCode.Forbidden);

        if (!await _flagRepository.ExistsById(flagId))
            throw new AppException("Flag não encontrado", HttpStatusCode.NotFound);

        if (!await _directoryFlagRepository.ExistsByDirectoryAndFlag(directoryId, flagId))
            throw new AppException("Diretório não possui essa flag", HttpStatusCode.Conflict);

        return await _directoryFlagRepository.Remove(new DirectoryFlag { DirectoryId = directoryId, FlagId = flagId });
    }
}
