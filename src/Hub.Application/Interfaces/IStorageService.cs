using Hub.Domain.DTOs;
using Hub.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Hub.Application.Interfaces;

public interface IStorageService
{
    Task<FileE> Upload(IFormFile formFile, FileE fileE);
    Task<FileE> Download(string fileId, UserDTO user);
    Task<bool> Move(MoveDTO moveDTO, UserDTO user);
    Task<bool> Delete(string fileId, UserDTO user);
}
