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

    /// <summary>
    /// Gets all flags.
    /// </summary>
    [HttpGet]
    public async Task<List<Flag>> Get() =>
        await _flagService.GetAll();

    /// <summary>
    /// Gets a flag by its ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id) =>
        Ok(await _flagService.GetById(id));

    /// <summary>
    /// Gets the count of flags.
    /// </summary>
    [HttpGet("count")]
    public async Task<IActionResult> GetCount() =>
        Ok(await _flagService.GetCount());

    /// <summary>
    /// Creates a new flag.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Flag flag)
    {
        var user = (UserDTO)HttpContext.Items["CurrentUserDTO"]!;
        flag.UserId = user.Id!;
        return Ok(await _flagService.Create(flag));
    }

    /// <summary>
    /// Updates an existing flag.
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> Put([FromBody] Flag flag)
    {
        var user = (UserDTO)HttpContext.Items["CurrentUserDTO"]!;
        flag.UserId = user.Id!;
        return Ok(await _flagService.Update(flag));
    }

    /// <summary>
    /// Deletes a flag by its ID.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var user = (UserDTO)HttpContext.Items["CurrentUserDTO"]!;
        return Ok(await _flagService.Delete(id, user));
    }
}
