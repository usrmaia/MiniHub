using Hub.API.InputModels;
using Hub.Domain.DTOs;
using Hub.Domain.Exceptions;
using Hub.Test.Global;
using System.Net;

namespace Hub.Test.API;

public class AuthControllerTest : GlobalClientRequest
{
    [Fact]
    public async Task Post_Login_Desenvolvedor_ReturnsToken()
    {
        var login = new LoginIM { UserName = "dev", Password = "!23L6(bNi.22T71,%4vfR{<~tA.]" };

        var response = await PostFromBody<AuthToken>(_loginClient, login);

        Assert.NotNull(response.AccessToken);
        Assert.NotNull(response.RefreshToken);
    }

    [Fact]
    public async Task Post_Login_Administrador_ReturnsToken()
    {
        var login = new LoginIM { UserName = "admin", Password = "!23L6(bNi.22T71,%4vfR{<~tA.]" };

        var response = await PostFromBody<AuthToken>(_loginClient, login);

        Assert.NotNull(response.AccessToken);
        Assert.NotNull(response.RefreshToken);
    }

    [Fact]
    public async Task Post_Login_Supervisor_ReturnsToken()
    {
        var login = new LoginIM { UserName = "super", Password = "!23L6(bNi.22T71,%4vfR{<~tA.]" };

        var response = await PostFromBody<AuthToken>(_loginClient, login);

        Assert.NotNull(response.AccessToken);
        Assert.NotNull(response.RefreshToken);
    }

    [Fact]
    public async Task Post_Login_ValidLogin_ReturnsToken()
    {
        var user = await GetUser();
        var login = new LoginIM { UserName = user.UserName, Password = user.Password };

        var response = await PostFromBody<AuthToken>(_loginClient, login);

        Assert.NotNull(response.AccessToken);
        Assert.NotNull(response.RefreshToken);
    }

    [Fact]
    public async Task Post_Login_InvalidUserName_ReturnsUnauthorized()
    {
        var user = await GetUser();
        var login = new LoginIM { UserName = new Bogus.Faker().Internet.UserName(), Password = user.Password };

        var response = await PostFromBody<AppException>(_loginClient, login);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Post_Login_InvalidPassword_ReturnsUnauthorized()
    {
        var user = await GetUser();
        var login = new LoginIM { UserName = user.UserName, Password = new Bogus.Faker().Internet.Password() };

        var response = await PostFromBody<AppException>(_loginClient, login);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Get_RefreshToken_ValidToken_ReturnsToken()
    {
        var token = await GetToken();
        _acessToken = token.RefreshToken;

        var response = await Get<AuthToken>(_refreshTokenClient);

        Assert.NotNull(response.AccessToken);
        Assert.NotNull(response.RefreshToken);
    }

    [Fact]
    public async Task Get_RefreshToken_InvalidToken_ReturnsBadRequest()
    {
        _acessToken = new Bogus.Faker().Internet.Password();

        var response = await Get<AppException>(_refreshTokenClient);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Get_User_ValidToken_ReturnsUser()
    {
        var token = await GetToken();
        _acessToken = token.AccessToken;

        var response = await Get<UserDTO>(_authUserClient);

        Assert.NotNull(response.Id);
        Assert.NotNull(response.UserName);
        Assert.NotNull(response.Email);
        Assert.NotNull(response.PhoneNumber);
        Assert.NotNull(response.Roles);
    }

    [Fact]
    public async Task Get_User_InvalidToken_ReturnsUnauthorized()
    {
        _acessToken = new Bogus.Faker().Internet.Password();

        await Assert.ThrowsAsync<Exception>(() => Get<UserDTO>(_authUserClient));
    }
}
