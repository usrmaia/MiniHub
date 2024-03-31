using Hub.API.ServiceFilters;
using Hub.Application.Interfaces;
using Hub.Domain.DTOs;
using Hub.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Hub.API;

[ApiController]
[Route("api/[controller]")]
[ServiceFilter(typeof(AuthAndUserExtractionFilter))]
public class DirectoryController : ControllerBase
{
    private readonly IDirectoryService _directoryService;
    private readonly IDirectoryFlagService _directoryFlagService;
    private readonly IDirectoryRoleService _directoryRoleService;

    public DirectoryController(IDirectoryService directoryservice,
        IDirectoryFlagService directoryFlagService,
        IDirectoryRoleService directoryRoleService)
    {
        _directoryService = directoryservice;
        _directoryFlagService = directoryFlagService;
        _directoryRoleService = directoryRoleService;
    }

    /// <summary>
    /// Retrieves all directories.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> representing the response of the operation.</returns>
    [HttpGet]
    public async Task<IActionResult> Get() =>
        Ok(await _directoryService.GetAll());

    /// <summary>
    /// Retrieves a directory by its ID.
    /// </summary>
    /// <param name="id">The ID of the directory to retrieve.</param>
    /// <returns>An <see cref="IActionResult"/> representing the response of the operation.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id) =>
        Ok(await _directoryService.GetById(id));

    /// <summary>
    /// Creates a new directory.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] DirectoryE directory)
    {
        var user = (UserDTO)HttpContext.Items["CurrentUserDTO"]!;
        directory.UserId = user.Id!;
        return Ok(await _directoryService.Create(directory));
    }

    /// <summary>
    /// Updates an existing directory.
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> Put(DirectoryE directory)
    {
        var user = (UserDTO)HttpContext.Items["CurrentUserDTO"]!;
        directory.UserId = user.Id!;
        return Ok(await _directoryService.Update(directory));
    }

    /// <summary>
    /// Deletes a directory by its ID.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var user = (UserDTO)HttpContext.Items["CurrentUserDTO"]!;
        return Ok(await _directoryService.Delete(id, user));
    }

    /// <summary>
    /// Adds a flag to a directory.
    /// </summary>
    [HttpPost("add-flag")]
    public async Task<IActionResult> AddFlag([FromBody] DirectoryFlag directoryFlag)
    {
        var user = (UserDTO)HttpContext.Items["CurrentUserDTO"]!;
        return Ok(await _directoryFlagService.AddFlag(directoryFlag.DirectoryId, directoryFlag.FlagId, user));
    }

    /// <summary>
    /// Adds a role to a directory.
    /// </summary>
    [HttpPost("add-role")]
    public async Task<IActionResult> AddRole([FromBody] DirectoryRole directoryRole)
    {
        var user = (UserDTO)HttpContext.Items["CurrentUserDTO"]!;
        return Ok(await _directoryRoleService.AddRole(directoryRole.DirectoryId, directoryRole.RoleId, user));
    }

    /// <summary>
    /// Removes a flag from a directory.
    /// </summary>
    [HttpDelete("remove-flag")]
    public async Task<IActionResult> RemoveFlag([FromBody] DirectoryFlag directoryFlag)
    {
        var user = (UserDTO)HttpContext.Items["CurrentUserDTO"]!;
        return Ok(await _directoryFlagService.RemoveFlag(directoryFlag.DirectoryId, directoryFlag.FlagId, user));
    }

    /// <summary>
    /// Removes a role from a directory.
    /// </summary>
    [HttpDelete("remove-role")]
    public async Task<IActionResult> RemoveRole([FromBody] DirectoryRole directoryRole)
    {
        var user = (UserDTO)HttpContext.Items["CurrentUserDTO"]!;
        return Ok(await _directoryRoleService.RemoveRole(directoryRole.DirectoryId, directoryRole.RoleId, user));
    }
}
