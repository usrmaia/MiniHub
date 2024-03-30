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

    [HttpGet]
    public async Task<IActionResult> Get() =>
        Ok(await _directoryService.GetAll());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id) =>
        Ok(await _directoryService.GetById(id));

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] DirectoryE directory)
    {
        var user = (UserDTO)HttpContext.Items["CurrentUserDTO"]!;
        directory.UserId = user.Id!;
        return Ok(await _directoryService.Create(directory));
    }

    [HttpPut]
    public async Task<IActionResult> Put(DirectoryE directory)
    {
        var user = (UserDTO)HttpContext.Items["CurrentUserDTO"]!;
        directory.UserId = user.Id!;
        return Ok(await _directoryService.Update(directory));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var user = (UserDTO)HttpContext.Items["CurrentUserDTO"]!;
        return Ok(await _directoryService.Delete(id, user));
    }

    [HttpPost("add-flag")]
    public async Task<IActionResult> AddFlag([FromBody] DirectoryFlag directoryFlag)
    {
        var user = (UserDTO)HttpContext.Items["CurrentUserDTO"]!;
        return Ok(await _directoryFlagService.AddFlag(directoryFlag.DirectoryId, directoryFlag.FlagId, user));
    }

    [HttpPost("add-role")]
    public async Task<IActionResult> AddRole([FromBody] DirectoryRole directoryRole)
    {
        var user = (UserDTO)HttpContext.Items["CurrentUserDTO"]!;
        return Ok(await _directoryRoleService.AddRole(directoryRole.DirectoryId, directoryRole.RoleId, user));
    }

    [HttpDelete("remove-flag")]
    public async Task<IActionResult> RemoveFlag([FromBody] DirectoryFlag directoryFlag)
    {
        var user = (UserDTO)HttpContext.Items["CurrentUserDTO"]!;
        return Ok(await _directoryFlagService.RemoveFlag(directoryFlag.DirectoryId, directoryFlag.FlagId, user));
    }

    [HttpDelete("remove-role")]
    public async Task<IActionResult> RemoveRole([FromBody] DirectoryRole directoryRole)
    {
        var user = (UserDTO)HttpContext.Items["CurrentUserDTO"]!;
        return Ok(await _directoryRoleService.RemoveRole(directoryRole.DirectoryId, directoryRole.RoleId, user));
    }
}
