using Hub.API.ServiceFilters;
using Hub.Application.Interfaces;
using Hub.Domain.DTOs;
using Hub.Domain.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Hub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[ServiceFilter(typeof(AuthAndUserExtractionFilter))]
public class HubController : ControllerBase
{
    private readonly IHubService _hubService;

    public HubController(IHubService hubService) =>
        _hubService = hubService;

    [HttpGet]
    public async Task<ActionResult<Items>> Query([FromQuery] ItemFilter filter) =>
        Ok(await _hubService.Query(filter));
}
