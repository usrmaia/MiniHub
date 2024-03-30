using Hub.Application.Interfaces;
using Hub.Domain.DTOs;
using Hub.Domain.Entities;
using Hub.Domain.Exceptions;
using Hub.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace Hub.Application.Services;

public class StorageService : IStorageService
{
    private readonly IFileRepository _fileRepository;
    private readonly IDirectoryRepository _directoryRepository;

    public StorageService(IFileRepository fileRepository, IDirectoryRepository directoryRepository)
    {
        _fileRepository = fileRepository;
        _directoryRepository = directoryRepository;
    }

    public async Task<FileE> Upload(IFormFile formFile, FileE fileE)
    {
        fileE.Name = formFile.FileName;
        fileE.Length = formFile.Length;
        fileE.Type = formFile.ContentType;

        var parentPath = new List<string>();
        if (!fileE.DirectoryId.IsNullOrEmpty())
            parentPath = await _directoryRepository.GetParentsNameById(fileE.DirectoryId!);

        var relativePath = Path.Combine(parentPath.ToArray());
        var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "hub", relativePath);

        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        var filePath = Path.Combine(directoryPath, fileE.Name);

        var stream = new FileStream(filePath, FileMode.Create);
        await formFile.CopyToAsync(stream);
        stream.Close();

        fileE.Path = relativePath;

        return await _fileRepository.Add(fileE);
    }

    public async Task<FileE> Download(string fileId, UserDTO user)
    {
        if (!await _fileRepository.ExistsById(fileId))
            throw new AppException("Arquivo não encontrado", HttpStatusCode.NotFound);

        var fileE = await _fileRepository.GetById(fileId);

        if (!fileE.Roles.IntersectBy(user.Roles, fr => fr.Name, StringComparer.OrdinalIgnoreCase).Any() &&
            fileE.UserId != user.Id)
            throw new AppException("Usuário não tem permissão para acessar o arquivo", HttpStatusCode.Forbidden);

        return fileE;
    }

    public async Task<bool> Move(MoveDTO moveDTO, UserDTO user)
    {
        if (!await _fileRepository.ExistsById(moveDTO.FileId))
            throw new AppException("Arquivo não encontrado", HttpStatusCode.NotFound);

        var fileE = await _fileRepository.GetById(moveDTO.FileId);

        if (fileE.UserId != user.Id)
            throw new AppException("Usuário não tem permissão para mover o arquivo", HttpStatusCode.Forbidden);

        if (!await _directoryRepository.ExistsById(moveDTO.DirectoryId))
            throw new AppException("Diretório não encontrado", HttpStatusCode.NotFound);

        var directory = await _directoryRepository.GetById(moveDTO.DirectoryId);

        if (directory.UserId != user.Id)
            throw new AppException("Usuário não tem permissão para mover o arquivo para esse diretório", HttpStatusCode.Forbidden);

        var parentFromPath = new List<string>();
        if (!fileE.DirectoryId.IsNullOrEmpty())
            parentFromPath = await _directoryRepository.GetParentsNameById(fileE.DirectoryId!);

        var parentToPath = new List<string>();
        if (!directory.Id.IsNullOrEmpty())
            parentToPath = await _directoryRepository.GetParentsNameById(directory.Id!);

        var relativeFromPath = Path.Combine(parentFromPath.ToArray());
        var relativeToPath = Path.Combine(parentToPath.ToArray());

        var fileFrom = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "hub", relativeFromPath, fileE.Name);
        var directoryTo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "hub", relativeToPath);
        var fileTo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "hub", relativeToPath, fileE.Name);

        if (!File.Exists(fileFrom))
            throw new AppException("Arquivo não encontrado", HttpStatusCode.NotFound);

        if (!Directory.Exists(directoryTo))
            Directory.CreateDirectory(directoryTo);

        if (File.Exists(fileTo))
            throw new AppException("Já existe um arquivo com esse nome no diretório de destino", HttpStatusCode.Conflict);

        File.Move(fileFrom, fileTo);

        fileE.DirectoryId = directory.Id;
        fileE.Path = relativeToPath;

        await _fileRepository.Update(fileE);

        return true;
    }

    public async Task<bool> Delete(string fileId, UserDTO user)
    {
        if (!await _fileRepository.ExistsById(fileId))
            throw new AppException("Arquivo não encontrado", HttpStatusCode.NotFound);

        var fileE = await _fileRepository.GetById(fileId);

        if (fileE.UserId != user.Id)
            throw new AppException("Usuário não tem permissão para deletar o arquivo", HttpStatusCode.Forbidden);

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "hub", fileE.Path, fileE.Name);

        if (!File.Exists(filePath))
            throw new AppException("Arquivo não encontrado", HttpStatusCode.NotFound);

        File.Delete(filePath);

        return true;
    }
}
