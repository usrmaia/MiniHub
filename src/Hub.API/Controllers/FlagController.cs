using Hub.API.ServiceFilters;
using Hub.Application.Interfaces;
using Hub.Domain.DTOs;
using Hub.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Hub.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[ServiceFilter(typeof(AuthAndUserExtractionFilter))]
public class FlagController : ControllerBase
{
    private readonly IFlagService _flagService;

    public FlagController(IFlagService flagService) =>
        _flagService = flagService;

    [HttpGet]
    public async Task<List<Flag>> Get() =>
        await _flagService.GetAll();

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id) =>
        Ok(await _flagService.GetById(id));

    [HttpGet("count")]
    public async Task<IActionResult> GetCount() =>
        Ok(await _flagService.GetCount());

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Flag flag)
    {
        var user = (UserDTO)HttpContext.Items["CurrentUserDTO"]!;
        flag.UserId = user.Id!;
        return Ok(await _flagService.Create(flag));
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] Flag flag)
    {
        var user = (UserDTO)HttpContext.Items["CurrentUserDTO"]!;
        flag.UserId = user.Id!;
        return Ok(await _flagService.Update(flag));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var user = (UserDTO)HttpContext.Items["CurrentUserDTO"]!;
        return Ok(await _flagService.Delete(id, user));
    }
}
