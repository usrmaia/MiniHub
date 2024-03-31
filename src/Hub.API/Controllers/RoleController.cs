using AutoMapper;
using Hub.Domain.DTOs;
using Hub.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MyApp.Namespace;

[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;
    private readonly IMapper _mapper;

    public RoleController(IRoleService roleService, IMapper mapper)
    {
        _roleService = roleService;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets all roles.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Desenvolvedor,Administrador,Supervisor")]
    public async Task<IActionResult> Get() =>
        Ok(await _roleService.Get());

    /// <summary>
    /// Creates a new role.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Desenvolvedor,Administrador,Supervisor")]
    public async Task<IActionResult> Post([FromBody] RoleDTO role)
    {
        var identityRole = _mapper.Map<IdentityRole>(role);
        var roleCreated = await _roleService.Create(identityRole);
        return Ok(_mapper.Map<RoleDTO>(roleCreated));
    }

    /// <summary>
    /// Updates an existing role.
    /// </summary>
    [HttpPut]
    [Authorize(Roles = "Desenvolvedor,Administrador,Supervisor")]
    public async Task<IActionResult> Put([FromBody] RoleDTO role)
    {
        var identityRole = _mapper.Map<IdentityRole>(role);
        var roleUpdated = await _roleService.Update(identityRole);
        return Ok(_mapper.Map<RoleDTO>(roleUpdated));
    }

    /// <summary>
    /// Deletes a role by its ID.
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Desenvolvedor,Administrador")]
    public async Task<IActionResult> Delete(string id)
    {
        await _roleService.Delete(id);
        return Ok();
    }
}
