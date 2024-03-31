using Hub.Domain.Entities;
using Hub.Domain.Exceptions;
using Hub.Test.Fakers;
using Hub.Test.Global;
using System.Net;

namespace Hub.Test.API;

public class DirectoryControllerTest : GlobalClientRequest
{
    [Fact]
    public async Task Get_ValidDirectory_ReturnsOk()
    {
        var directory = await GetDirectory();

        var response = await GetFromUri<DirectoryE>(_directoryClient, directory.Id!);

        Assert.Equal(directory.Name, response.Name);
    }

    [Fact]
    public async Task Get_InvalidDirectory_ReturnsNotFound()
    {
        var response = await GetFromUri<AppException>(_directoryClient, Guid.NewGuid().ToString());

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Post_ValidDirectory_ReturnsOk()
    {
        var directory = new DirectoryFake().Generate();

        var response = await PostFromBody<DirectoryE>(_directoryClient, directory);

        Assert.Equal(directory.Name, response.Name);
    }

    [Fact]
    public async Task Post_ValidDirectoryWithParent_ReturnsOk()
    {
        var directory = await GetDirectory();
        var directoryWithParent = new DirectoryFake(parentId: directory.Id).Generate();

        var response = await PostFromBody<DirectoryE>(_directoryClient, directoryWithParent);

        Assert.Equal(directoryWithParent.Name, response.Name);
    }

    [Fact]
    public async Task Post_InvalidDirectoryParent_ReturnsNotFound()
    {
        var directory = new DirectoryFake(parentId: Guid.NewGuid().ToString()).Generate();

        var response = await PostFromBody<AppException>(_directoryClient, directory);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Post_InvalidDirectoryWithSameName_ReturnsConflict()
    {
        var parent = await GetDirectory();
        var directory = await GetDirectory(parentId: parent.Id);
        var directoryWithSameName = new DirectoryFake(name: directory.Name, parentId: directory.ParentId).Generate();

        var response = await PostFromBody<AppException>(_directoryClient, directoryWithSameName);

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task Put_ValidDirectory_ReturnsOk()
    {
        var parent = await GetDirectory();
        var directory = await GetDirectory(parentId: parent.Id);
        directory.Description = new Bogus.Faker().Lorem.Sentence();

        var response = await PutFromBody<DirectoryE>(_directoryClient, directory);

        Assert.Equal(directory.ParentId, response.ParentId);
        Assert.Equal(directory.Description, response.Description);
    }

    [Fact]
    public async Task Put_InvalidDirectory_ReturnsNotFound()
    {
        var directory = await GetDirectory();
        directory.Id = Guid.NewGuid().ToString();

        var response = await PutFromBody<AppException>(_directoryClient, directory);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ValidDirectory_ReturnsOk()
    {
        var directory = await GetDirectory();

        var response = await DeleteFromUri<DirectoryE>(_directoryClient, directory.Id!);

        Assert.Equal(directory.Name, response.Name);
    }

    [Fact]
    public async Task Delete_InvalidDirectory_ReturnsNotFound()
    {
        var response = await DeleteFromUri<AppException>(_directoryClient, Guid.NewGuid().ToString());

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_NoDeleteCascade_ReturnsOk()
    {
        var parent = await GetDirectory();
        var directory = await GetDirectory(parentId: parent.Id);

        var deletedDirectory = await DeleteFromUri<DirectoryE>(_directoryClient, directory.Id!);
        var response = await GetFromUri<DirectoryE>(_directoryClient, parent.Id!);

        Assert.Equal(parent.Name, response.Name);
    }

    [Fact]
    public async Task Delete_DeleteCascade_ReturnsOk()
    {
        var parent = await GetDirectory();
        var directory = await GetDirectory(parentId: parent.Id);

        var deletedParent = await DeleteFromUri<DirectoryE>(_directoryClient, parent.Id!);
        var response = await GetFromUri<AppException>(_directoryClient, directory.Id!);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Post_AddFlag_ValidDirectoryFlag_ReturnsOk()
    {
        var directory = await GetDirectory();
        var flag = await GetFlag();
        var directoryFlag = new DirectoryFlag { DirectoryId = directory.Id!, FlagId = flag.Id! };

        var result = await PostFromBody<DirectoryFlag>(_addDirectoryToFlagClient, directoryFlag);

        Assert.Equal(directoryFlag.DirectoryId, result.DirectoryId);
        Assert.Equal(directoryFlag.FlagId, result.FlagId);
    }

    [Fact]
    public async Task Post_AddFlag_InvalidDirectory_ReturnsNotFound()
    {
        var flag = await GetFlag();
        var directoryFlag = new DirectoryFlag { DirectoryId = Guid.NewGuid().ToString(), FlagId = flag.Id! };

        var response = await PostFromBody<AppException>(_addDirectoryToFlagClient, directoryFlag);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Post_AddFlag_InvalidFlag_ReturnsNotFound()
    {
        var directory = await GetDirectory();
        var directoryFlag = new DirectoryFlag { DirectoryId = directory.Id!, FlagId = Guid.NewGuid().ToString() };

        var response = await PostFromBody<AppException>(_addDirectoryToFlagClient, directoryFlag);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Post_AddFlag_InvalidDirectoryFlag_ReturnsConflict()
    {
        var directoryFlag = await GetDirectoryFlag();

        var response = await PostFromBody<AppException>(_addDirectoryToFlagClient, directoryFlag);

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task Post_AddRole_ValidDirectoryRole_ReturnsOk()
    {
        var directory = await GetDirectory();
        var role = await GetRole();
        var directoryRole = new DirectoryRole { DirectoryId = directory.Id!, RoleId = role.Id! };

        var result = await PostFromBody<DirectoryRole>(_addDirectoryToRoleClient, directoryRole);

        Assert.Equal(directoryRole.DirectoryId, result.DirectoryId);
        Assert.Equal(directoryRole.RoleId, result.RoleId);
    }

    [Fact]
    public async Task Post_AddRole_InvalidDirectory_ReturnsNotFound()
    {
        var role = await GetRole();
        var directoryRole = new DirectoryRole { DirectoryId = Guid.NewGuid().ToString(), RoleId = role.Id! };

        var response = await PostFromBody<AppException>(_addDirectoryToRoleClient, directoryRole);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Post_AddRole_InvalidRole_ReturnsNotFound()
    {
        var directory = await GetDirectory();
        var directoryRole = new DirectoryRole { DirectoryId = directory.Id!, RoleId = Guid.NewGuid().ToString() };

        var response = await PostFromBody<AppException>(_addDirectoryToRoleClient, directoryRole);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Post_AddRole_InvalidDirectoryRole_ReturnsConflict()
    {
        var directoryRole = await GetDirectoryRole();

        var response = await PostFromBody<AppException>(_addDirectoryToRoleClient, directoryRole);

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task Delete_RemoveFlag_ValidDirectoryFlag_ReturnsOk()
    {
        var directoryFlag = await GetDirectoryFlag();

        var response = await DeleteFromBody<DirectoryFlag>(_removeDirectoryFromFlagClient, directoryFlag);

        Assert.Equal(directoryFlag.DirectoryId, response.DirectoryId);
        Assert.Equal(directoryFlag.FlagId, response.FlagId);
    }

    [Fact]
    public async Task Delete_RemoveFlag_InvalidDirectory_ReturnsNotFound()
    {
        var flag = await GetFlag();
        var directoryFlag = new DirectoryFlag { DirectoryId = Guid.NewGuid().ToString(), FlagId = flag.Id! };

        var response = await DeleteFromBody<AppException>(_removeDirectoryFromFlagClient, directoryFlag);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_RemoveFlag_InvalidFlag_ReturnsNotFound()
    {
        var directory = await GetDirectory();
        var directoryFlag = new DirectoryFlag { DirectoryId = directory.Id!, FlagId = Guid.NewGuid().ToString() };

        var response = await DeleteFromBody<AppException>(_removeDirectoryFromFlagClient, directoryFlag);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_RemoveRole_ValidDirectoryRole_ReturnsOk()
    {
        var directoryRole = await GetDirectoryRole();

        var response = await DeleteFromBody<DirectoryRole>(_removeDirectoryFromRoleClient, directoryRole);

        Assert.Equal(directoryRole.DirectoryId, response.DirectoryId);
        Assert.Equal(directoryRole.RoleId, response.RoleId);
    }

    [Fact]
    public async Task Delete_RemoveRole_InvalidDirectory_ReturnsNotFound()
    {
        var role = await GetRole();
        var directoryRole = new DirectoryRole { DirectoryId = Guid.NewGuid().ToString(), RoleId = role.Id! };

        var response = await DeleteFromBody<AppException>(_removeDirectoryFromRoleClient, directoryRole);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_RemoveRole_InvalidRole_ReturnsNotFound()
    {
        var directory = await GetDirectory();
        var directoryRole = new DirectoryRole { DirectoryId = directory.Id!, RoleId = Guid.NewGuid().ToString() };

        var response = await DeleteFromBody<AppException>(_removeDirectoryFromRoleClient, directoryRole);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
