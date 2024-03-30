using System.Net;
using Hub.API.Utils;
using Hub.Application.Interfaces;
using Hub.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hub.API.ServiceFilters;

public class AuthAndUserExtractionFilter : IAsyncActionFilter
{
    private readonly IAuthService _authService;

    public AuthAndUserExtractionFilter(IAuthService authService) =>
        _authService = authService;

    public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Headers.ContainsKey("Authorization"))
            throw new AppException("Token não encontrado", HttpStatusCode.Unauthorized);

        var auth = context.HttpContext.Request.Headers.Authorization.ToString();
        auth = AuthUtil.ExtractTokenFromHeader(auth);
        var user = _authService.GetUser(auth).Result;

        context.HttpContext.Items.Add("CurrentUserDTO", user);

        return next();
    }
}
