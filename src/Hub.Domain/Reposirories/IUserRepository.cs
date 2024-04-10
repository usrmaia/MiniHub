﻿using Hub.Domain.DTOs;
using Hub.Domain.Filters;
using Microsoft.AspNetCore.Identity;

namespace Hub.Domain.Repositories;

public interface IUserRepository : IBaseRepository<IdentityUser>
{
    Task<QueryResult<UserDTO>> Query(UserFilter filter);
    Task<IdentityUser> Auth(string userName, string password);
    bool IsDefaultUser(IdentityUser user);
    Task<bool> ExistsById(string id);
    Task<bool> ExistsByUserName(string userName);
    Task<bool> ExistsByEmail(string email);
    Task<bool> ExistsByPhoneNumber(string phoneNumber);
    Task<IdentityUser> GetById(string id);
    Task<IdentityUser> GetByUserName(string userName);
    Task<IdentityUser> GetByEmail(string email);
    Task<IdentityUser> GetByPhoneNumber(string phoneNumber);
    Task<List<string>> GetRoles(IdentityUser user);
    Task<bool> CheckPassword(string id, string password);
    Task<bool> CheckPassword(IdentityUser user, string password);
    Task<IdentityUser> Add(IdentityUser user, string password);
    new Task<IdentityUser> Update(IdentityUser user);
    Task<bool> UpdatePassword(string id, string oldPassword, string newPassword);
    Task<bool> RemoveByUserName(string userName);
}
