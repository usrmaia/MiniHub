using Hub.Domain.DTOs;

namespace Hub.Application.Interfaces;

public interface IAuthService
{
    Task<AuthToken> Login(string userName, string password);
    Task<AuthToken> RefreshToken(string refreshToken);
    Task<UserDTO> GetUser(string acessToken);
}
