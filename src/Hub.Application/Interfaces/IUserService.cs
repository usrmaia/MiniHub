using Hub.Domain.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Hub.Application.Interfaces;

public interface IUserService
{
    Task<List<UserDTO>> Get();
    Task<UserDTO> GetByUserName(string userName);
    Task<IdentityUser> GetById(string id);
    Task<IdentityUser> Create(IdentityUser user, string password);
    Task<IdentityUser> Update(IdentityUser user, string userId);
    Task<bool> UpdatePassword(string userId, string oldPassword, string newPassword);
    Task<IdentityUser> Delete(string id);
}
