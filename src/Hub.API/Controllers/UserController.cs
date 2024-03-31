using AutoMapper;
using Hub.Domain.DTOs;
using Hub.API.InputModels;
using Hub.API.ServiceFilters;
using Hub.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MyApp.Namespace;

[Route("api/[controller]")]
[ApiController]
[ServiceFilter(typeof(AuthAndUserExtractionFilter))]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserRoleService _userRoleService;
    private readonly IMapper _mapper;

    public UserController(IUserService userService,
        IUserRoleService userRoleService,
        IMapper mapper)
    {
        _userService = userService;
        _userRoleService = userRoleService;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets all users.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Desenvolvedor,Administrador,Supervisor")]
    public async Task<IActionResult> Get() =>
        Ok(await _userService.Get());

    /// <summary>
    /// Gets a user by ID.
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Desenvolvedor,Administrador,Supervisor")]
    public async Task<IActionResult> GetById(string id)
    {
        var identityUser = await _userService.GetById(id);
        return Ok(_mapper.Map<UserDTO>(identityUser));
    }

    /// <summary>
    /// Gets a user by username.
    /// </summary>
    [HttpGet("username/{userName}")]
    [Authorize(Roles = "Desenvolvedor,Administrador,Supervisor")]
    public async Task<IActionResult> Get(string userName)
    {
        var identityUser = await _userService.GetByUserName(userName);
        return Ok(_mapper.Map<UserDTO>(identityUser));
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Desenvolvedor,Administrador,Supervisor")]
    public async Task<IActionResult> Post([FromBody] UserDTO user)
    {
        var identityUser = _mapper.Map<IdentityUser>(user);
        var userCreated = await _userService.Create(identityUser, user.Password);
        return Ok(_mapper.Map<UserDTO>(userCreated));
    }

    /// <summary>
    /// Updates a user.
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> Put([FromBody] UserDTO user)
    {
        var userDTO = (UserDTO)HttpContext.Items["CurrentUserDTO"]!;
        var identityUser = _mapper.Map<IdentityUser>(user);
        var userUpdated = await _userService.Update(identityUser, userDTO.Id!);
        return Ok(_mapper.Map<UserDTO>(userUpdated));
    }

    /// <summary>
    /// Updates the password of the current user.
    /// </summary>
    [HttpPut("password")]
    public async Task<IActionResult> Put([FromBody] UpdatePasswordIM model)
    {
        var user = (UserDTO)HttpContext.Items["CurrentUserDTO"]!;
        await _userService.UpdatePassword(user.Id!, model.OldPassword, model.Password);
        return Ok(user);
    }

    /// <summary>
    /// Deletes a user by ID.
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Desenvolvedor,Administrador")]
    public async Task<IActionResult> Delete(string id)
    {
        var identityUser = await _userService.Delete(id);
        return Ok(_mapper.Map<UserDTO>(identityUser));
    }

    /// <summary>
    /// Adds a user to a role.
    /// </summary>
    [HttpPost("add-to-role")]
    [Authorize(Roles = "Desenvolvedor,Administrador,Supervisor")]
    public async Task<IActionResult> AddToRole([FromBody] UserRoleIM model) =>
        Ok(await _userRoleService.AddToRole(model.UserId, model.RoleName));

    /// <summary>
    /// Removes a user from a role.
    /// </summary>
    [HttpDelete("remove-from-role")]
    [Authorize(Roles = "Desenvolvedor,Administrador,Supervisor")]
    public async Task<IActionResult> RemoveFromRole([FromBody] UserRoleIM model) =>
        Ok(await _userRoleService.RemoveFromRole(model.UserId, model.RoleName));
}
