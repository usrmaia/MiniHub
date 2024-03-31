using Hub.API.InputModels;
using Hub.API.ServiceFilters;
using Hub.Application.Interfaces;
using Hub.Domain.DTOs;
using Hub.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[ServiceFilter(typeof(AuthAndUserExtractionFilter))]
public class StorageController : ControllerBase
{
    private readonly IStorageService _storageService;
    private readonly IFileFlagService _fileFlagService;
    private readonly IFileRoleService _fileRoleService;

    public StorageController(
        IStorageService storageService,
        IFileFlagService fileFlagService,
        IFileRoleService fileRoleService)
    {
        _storageService = storageService;
        _fileFlagService = fileFlagService;
        _fileRoleService = fileRoleService;
    }

    /// <summary>
    /// Uploads a file to the storage.
    /// </summary>
    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] PostFileIM model)
    {
        var user = (UserDTO)HttpContext.Items["CurrentUserDTO"]!;
        model.FileE!.UserId = user.Id!;
        return Ok(await _storageService.Upload(model.FormFile, model.FileE!));
    }

    /// <summary>
    /// Downloads a file from the storage.
    /// </summary>
    [HttpGet("download/{fileId}")]
    public async Task<IActionResult> Download(string fileId)
    {
        var user = (UserDTO)HttpContext.Items["CurrentUserDTO"]!;
        var file = await _storageService.Download(fileId, user);
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "hub", file.Path, file.Name);
        return PhysicalFile(path, "application/octet-stream", file.Name);
    }

    /// <summary>
    /// Moves a file to a different location in the storage.
    /// </summary>
    [Authorize]
    [HttpPost("move")]
    public async Task<IActionResult> Move([FromBody] MoveDTO moveDTO)
    {
        var user = (UserDTO)HttpContext.Items["CurrentUserDTO"]!;
        return Ok(await _storageService.Move(moveDTO, user));
    }

    /// <summary>
    /// Deletes a file from the storage.
    /// </summary>
    [Authorize]
    [HttpDelete("{fileId}")]
    public async Task<IActionResult> Delete(string fileId)
    {
        var user = (UserDTO)HttpContext.Items["CurrentUserDTO"]!;
        return Ok(await _storageService.Delete(fileId, user));
    }

    /// <summary>
    /// Adds a flag to a file.
    /// </summary>
    [Authorize]
    [HttpPost("add-flag")]
    public async Task<IActionResult> AddFlag([FromBody] FileFlag fileFlag)
    {
        var user = (UserDTO)HttpContext.Items["CurrentUserDTO"]!;
        return Ok(await _fileFlagService.AddFlag(fileFlag.FileId, fileFlag.FlagId, user));
    }

    /// <summary>
    /// Removes a flag from a file.
    /// </summary>
    [Authorize]
    [HttpDelete("remove-flag")]
    public async Task<IActionResult> RemoveFlag([FromBody] FileFlag fileFlag)
    {
        var user = (UserDTO)HttpContext.Items["CurrentUserDTO"]!;
        return Ok(await _fileFlagService.RemoveFlag(fileFlag.FileId, fileFlag.FlagId, user));
    }

    /// <summary>
    /// Adds a role to a file.
    /// </summary>
    [Authorize]
    [HttpPost("add-role")]
    public async Task<IActionResult> AddRole([FromBody] FileRole fileRole)
    {
        var user = (UserDTO)HttpContext.Items["CurrentUserDTO"]!;
        return Ok(await _fileRoleService.AddRole(fileRole.FileId, fileRole.RoleId, user));
    }

    /// <summary>
    /// Removes a role from a file.
    /// </summary>
    [Authorize]
    [HttpDelete("remove-role")]
    public async Task<IActionResult> RemoveRole([FromBody] FileRole fileRole)
    {
        var user = (UserDTO)HttpContext.Items["CurrentUserDTO"]!;
        return Ok(await _fileRoleService.RemoveRole(fileRole.FileId, fileRole.RoleId, user));
    }
}
