using Hub.Domain.DTOs;
using Hub.Domain.Exceptions;
using Hub.Test.Fakers;
using Hub.Test.Global;
using System.Net;

namespace Hub.Test.API;

public class RoleControllerTest : GlobalClientRequest
{
    [Fact]
    public async Task Post_ValidRole_ReturnsOkResult()
    {
        var role = new RoleFake().Generate();

        var result = await PostFromBody<RoleDTO>(_roleClient, role);

        Assert.Equivalent(role.Name, result.Name);
    }

    [Fact]
    public async Task Post_InvalidRoleWithExistingName_ReturnsConflictResult()
    {
        var role = await GetRole();
        var roleWithExistingName = new RoleFake(name: role.Name).Generate();

        var resultWithExistingName = await PostFromBody<AppException>(_roleClient, roleWithExistingName);

        Assert.Equal(HttpStatusCode.Conflict, resultWithExistingName.StatusCode);
    }

    [Fact]
    public async Task Put_ValidRole_ReturnsOkResult()
    {
        var role = await GetRole();
        var updatedRole = new RoleFake(id: role.Id).Generate();

        var result = await PutFromBody<RoleDTO>(_roleClient, updatedRole);

        Assert.Equivalent(updatedRole.Name, result.Name);
    }

    [Fact]
    public async Task Put_InvalidRoleWithNonExistingId_ReturnsNotFoundResult()
    {
        var role = new RoleFake(id: Guid.NewGuid().ToString()).Generate();

        var result = await PutFromBody<AppException>(_roleClient, role);

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }

    [Fact]
    public async Task Put_InvalidRoleWithExistingName_ReturnsConflictResult()
    {
        var role1 = await GetRole();
        var role2 = await GetRole();
        var roleWithExistingName = new RoleFake(id: role1.Id, name: role2.Name).Generate();

        var resultWithExistingName = await PutFromBody<AppException>(_roleClient, roleWithExistingName);

        Assert.Equal(HttpStatusCode.Conflict, resultWithExistingName.StatusCode);
    }

    [Fact]
    public async Task Delete_ValidRole_ReturnsOkResult()
    {
        var role = await GetRole();

        await Assert.ThrowsAsync<Exception>(() => DeleteFromUri<AppException>(_roleClient, role.Id!));
    }

    [Fact]
    public async Task Delete_InvalidRole_ReturnsNotFoundResult()
    {
        var result = await DeleteFromUri<AppException>(_roleClient, Guid.NewGuid().ToString());

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }
}
