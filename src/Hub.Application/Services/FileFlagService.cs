using Hub.Application.Interfaces;
using Hub.Domain.DTOs;
using Hub.Domain.Entities;
using Hub.Domain.Exceptions;
using Hub.Domain.Repositories;
using System.Net;

namespace Hub.Application.Services;

public class FileFlagService : IFileFlagService
{
    private readonly IFileRepository _fileRepository;
    private readonly IFlagRepository _flagRepository;
    private readonly IFileFlagRepository _fileFlagRepository;

    public FileFlagService(IFileRepository fileRepository,
        IFlagRepository flagRepository,
        IFileFlagRepository fileFlagRepository)
    {
        _fileRepository = fileRepository;
        _flagRepository = flagRepository;
        _fileFlagRepository = fileFlagRepository;
    }

    public async Task<FileFlag> AddFlag(string fileId, string flagId, UserDTO user)
    {
        if (!await _fileRepository.ExistsById(fileId))
            throw new AppException("Arquivo não encontrado", HttpStatusCode.NotFound);

        var file = await _fileRepository.GetById(fileId);

        if (file.UserId != user.Id)
            throw new AppException("Usuário não autorizado", HttpStatusCode.Unauthorized);

        if (!await _flagRepository.ExistsById(flagId))
            throw new AppException("Flag não encontrada", HttpStatusCode.NotFound);

        if (await _fileFlagRepository.ExistsByFileAndFlag(fileId, flagId))
            throw new AppException("Flag já adicionada ao arquivo", HttpStatusCode.Conflict);

        return await _fileFlagRepository.Add(new FileFlag { FileId = fileId, FlagId = flagId });
    }

    public async Task<FileFlag> RemoveFlag(string fileId, string flagId, UserDTO user)
    {
        if (!await _fileRepository.ExistsById(fileId))
            throw new AppException("Arquivo não encontrado", HttpStatusCode.NotFound);

        var file = await _fileRepository.GetById(fileId);

        if (file.UserId != user.Id)
            throw new AppException("Usuário não autorizado", HttpStatusCode.Unauthorized);

        if (!await _flagRepository.ExistsById(flagId))
            throw new AppException("Flag não encontrada", HttpStatusCode.NotFound);

        if (!await _fileFlagRepository.ExistsByFileAndFlag(fileId, flagId))
            throw new AppException("Flag não encontrada no arquivo", HttpStatusCode.Conflict);

        return await _fileFlagRepository.Remove(new FileFlag { FileId = fileId, FlagId = flagId });
    }
}
