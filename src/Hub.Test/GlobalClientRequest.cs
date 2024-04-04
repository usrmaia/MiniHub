using Hub.API.InputModels;
using Hub.Domain.DTOs;
using Hub.Domain.Entities;
using Hub.Test.Fakers;
using Hub.Test.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly)]

namespace Hub.Test.Global;

public class GlobalClientRequest : HttpClientUtil
{
    public const string BaseUrl = "http://localhost:5155/api/";
    public readonly HttpClient _authClient = new() { BaseAddress = new Uri($"{BaseUrl}Auth/") };
    public readonly HttpClient _loginClient = new() { BaseAddress = new Uri($"{BaseUrl}Auth/login") };
    public readonly HttpClient _refreshTokenClient = new() { BaseAddress = new Uri($"{BaseUrl}Auth/refresh-token") };
    public readonly HttpClient _authUserClient = new() { BaseAddress = new Uri($"{BaseUrl}Auth/user") };
    public readonly HttpClient _userClient = new() { BaseAddress = new Uri($"{BaseUrl}User/") };
    public readonly HttpClient _userPasswordClient = new() { BaseAddress = new Uri($"{BaseUrl}User/password/") };
    public readonly HttpClient _addUserToRoleClient = new() { BaseAddress = new Uri($"{BaseUrl}User/add-to-role/") };
    public readonly HttpClient _removeUserFromRoleClient = new() { BaseAddress = new Uri($"{BaseUrl}User/remove-from-role/") };
    public readonly HttpClient _roleClient = new() { BaseAddress = new Uri($"{BaseUrl}Role/") };
    public readonly HttpClient _flagClient = new() { BaseAddress = new Uri($"{BaseUrl}Flag/") };
    public readonly HttpClient _directoryClient = new() { BaseAddress = new Uri($"{BaseUrl}Directory/") };
    public readonly HttpClient _addDirectoryToFlagClient = new() { BaseAddress = new Uri($"{BaseUrl}Directory/add-flag/") };
    public readonly HttpClient _removeDirectoryFromFlagClient = new() { BaseAddress = new Uri($"{BaseUrl}Directory/remove-flag/") };
    public readonly HttpClient _addDirectoryToRoleClient = new() { BaseAddress = new Uri($"{BaseUrl}Directory/add-role/") };
    public readonly HttpClient _removeDirectoryFromRoleClient = new() { BaseAddress = new Uri($"{BaseUrl}Directory/remove-role/") };
    public readonly HttpClient _storageClient = new() { BaseAddress = new Uri($"{BaseUrl}Storage/") };
    public readonly HttpClient _uploadStorageClient = new() { BaseAddress = new Uri($"{BaseUrl}Storage/upload/") };
    public readonly HttpClient _downloadStorageClient = new() { BaseAddress = new Uri($"{BaseUrl}Storage/download/") };
    public readonly HttpClient _moveStorageClient = new() { BaseAddress = new Uri($"{BaseUrl}Storage/move/") };
    public readonly HttpClient _addFileToFlagClient = new() { BaseAddress = new Uri($"{BaseUrl}Storage/add-flag/") };
    public readonly HttpClient _removeFileFromFlagClient = new() { BaseAddress = new Uri($"{BaseUrl}Storage/remove-flag/") };
    public readonly HttpClient _addFileToRoleClient = new() { BaseAddress = new Uri($"{BaseUrl}Storage/add-role/") };
    public readonly HttpClient _removeFileFromRoleClient = new() { BaseAddress = new Uri($"{BaseUrl}Storage/remove-role/") };

    #region GetEntityFake

    public async Task<UserToken> GetToken(string? userId = null, string? userName = null, string? password = null)
    {
        var user = await GetUser(id: userId, userName: userName, password: password);
        var login = new LoginIM { UserName = user.UserName, Password = user.Password };
        return await PostFromBody<UserToken>(_loginClient, login);
    }

    public async Task<UserDTO> GetUser(string? id = null, string? userName = null, string? email = null, string? phoneNumber = null, string? password = null, List<string>? roles = null)
    {
        if (!id.IsNullOrEmpty())
        {
            var user = await GetFromUri<UserDTO>(_userClient, id!);
            user.Password = password ?? user.Password;
            return user;
        }

        var userFake = new UserFake(id: id, userName: userName, email: email, phoneNumber: phoneNumber, password: password, roles: roles).Generate();
        var newUser = await PostFromBody<UserDTO>(_userClient, userFake);
        newUser.Password = userFake.Password;

        return newUser;
    }

    public async Task<RoleDTO> GetRole(string? id = null, string? name = null)
    {
        if (!id.IsNullOrEmpty())
            return await GetFromUri<RoleDTO>(_roleClient, id!);

        var roleFake = new RoleFake(id, name).Generate();
        return await PostFromBody<RoleDTO>(_roleClient, roleFake);
    }

    public async Task<UserRoleIM> GetUserRole(string? userId = null, string? roleName = null)
    {
        var user = await GetUser(id: userId);
        var role = await GetRole(name: roleName);
        var model = new UserRoleIM { UserId = user.Id!, RoleName = role.Name };

        await PostFromBody<UserDTO>(_addUserToRoleClient, model);

        return model;
    }

    public async Task<Flag> GetFlag(string? id = null, string? name = null, string? description = null, DateTime? createdAt = null, DateTime? updatedAt = null, string? userId = null)
    {
        if (!id.IsNullOrEmpty())
            return await GetFromUri<Flag>(_flagClient, id!);

        var flagFake = new FlagFake(id: id, name: name, description: description, createdAt: createdAt, updatedAt: updatedAt).Generate();
        return await PostFromBody<Flag>(_flagClient, flagFake);
    }

    public async Task<DirectoryE> GetDirectory(string? id = null, string? parentId = null, string? name = null, string? description = null, DateTime? createdAt = null, DateTime? updatedAt = null, List<FileE>? files = null, List<Flag>? flags = null, List<IdentityRole>? roles = null, string? userId = null)
    {
        if (!id.IsNullOrEmpty())
            return await GetFromUri<DirectoryE>(_directoryClient, id!);

        var user = await GetUser(id: userId);
        var directoryFake = new DirectoryFake(id: id, parentId: parentId, name: name, description: description, createdAt: createdAt, updatedAt: updatedAt, files: files, flags: flags, roles: roles, userId: user.Id).Generate();
        return await PostFromBody<DirectoryE>(_directoryClient, directoryFake);
    }

    public async Task<DirectoryFlag> GetDirectoryFlag(string? directoryId = null, string? flagId = null)
    {
        var directory = await GetDirectory(directoryId);
        var flag = await GetFlag(flagId);
        var directoryFlag = new DirectoryFlag { DirectoryId = directory.Id!, FlagId = flag.Id! };

        return await PostFromBody<DirectoryFlag>(_addDirectoryToFlagClient, directoryFlag);
    }

    public async Task<DirectoryRole> GetDirectoryRole(string? directoryId = null, string? roleId = null)
    {
        var directory = await GetDirectory(directoryId);
        var role = await GetRole(roleId);
        var directoryRole = new DirectoryRole { DirectoryId = directory.Id!, RoleId = role.Id! };

        return await PostFromBody<DirectoryRole>(_addDirectoryToRoleClient, directoryRole);
    }

    public async Task<FileE> GetFile(string? id = null, string? directoryId = null, string? name = null, string? type = null, long? length = null, string? path = null, List<Flag>? flags = null, List<IdentityRole>? roles = null, string? userId = null)
    {
        if (!id.IsNullOrEmpty())
            return await GetFromUri<FileE>(_storageClient, id!);

        if (!userId.IsNullOrEmpty())
        {
            var user = await GetUser();
            userId = user.Id;
        }

        var formFile = new FormFileFake(fileName: name).Generate();
        var file = new FileFake(name: formFile.FileName, type: formFile.ContentType, length: formFile.Length, directoryId: directoryId, path: path, flags: flags, roles: roles, userId: userId).Generate();
        var model = new PostFileIM { FormFile = formFile, FileE = file };
        return await PostFromForm<FileE>(_uploadStorageClient, model);
    }

    public async Task<FileFlag> GetFileFlag(string? fileId = null, string? flagId = null)
    {
        var file = await GetFile(id: fileId);
        var flag = await GetFlag(id: flagId);
        var fileFlag = new FileFlag { FileId = file.Id!, FlagId = flag.Id! };

        return await PostFromBody<FileFlag>(_addFileToFlagClient, fileFlag);
    }

    public async Task<FileRole> GetFileRole(string? fileId = null, string? roleId = null)
    {
        var file = await GetFile(id: fileId);
        var role = await GetRole(id: roleId);
        var fileRole = new FileRole { FileId = file.Id!, RoleId = role.Id! };

        return await PostFromBody<FileRole>(_addFileToRoleClient, fileRole);
    }

    #endregion
}
