using Hub.API.InputModels;
using Hub.Application.Interfaces;
using Hub.API.Utils;
using Hub.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hub.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service) =>
        _service = service;

    /// <summary>
    /// Authenticates a user and returns an auth token.
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<AuthToken>> Login([FromBody] LoginIM login) =>
        await _service.Login(login.UserName, login.Password);

    /// <summary>
    /// Refreshes an auth token.
    /// </summary>
    [HttpGet("refresh-token")]
    public async Task<ActionResult<AuthToken>> RefreshToken([FromHeader(Name = "Authorization")] string auth)
    {
        var token = AuthUtil.ExtractTokenFromHeader(auth);
        return await _service.RefreshToken(token);
    }

    /// <summary>
    /// Retrieves the user information based on the provided authorization token. Note: Authorization token in the format "Bearer {token}".
    /// </summary>
    [Authorize]
    [HttpGet("user")]
    public async Task<ActionResult<UserDTO>> Get([FromHeader(Name = "Authorization")] string auth)
    {
        var token = AuthUtil.ExtractTokenFromHeader(auth);
        return await _service.GetUser(token);
    }
}
