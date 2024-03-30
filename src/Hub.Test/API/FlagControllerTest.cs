using Hub.Domain.Entities;
using Hub.Domain.Exceptions;
using Hub.Test.Fakers;
using Hub.Test.Global;
using System.Net;

namespace Hub.Test.API;

public class FlagControllerTest : GlobalClientRequest
{
    [Fact]
    public async Task GetById_ValidFlag_ReturnsOkResult()
    {
        var flag = await GetFlag();

        var result = await GetFromUri<Flag>(_flagClient, flag.Id!);

        Assert.Equal(flag.Name, result.Name);
    }

    [Fact]
    public async Task GetById_InvalidId_ReturnsNotFound()
    {
        var result = await GetFromUri<AppException>(_flagClient, Guid.NewGuid().ToString());

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }

    [Fact]
    public async Task GetCount_ReturnsOkResult()
    {
        var beforeCount = await GetCount(_flagClient);
        var flag = await GetFlag();
        var afterCount = await GetCount(_flagClient);

        Assert.Equal(beforeCount + 1, afterCount);
    }

    [Fact]
    public async Task Post_ValidFlag_ReturnsOkResult()
    {
        var flag = new FlagFake().Generate();

        var result = await PostFromBody<Flag>(_flagClient, flag);

        Assert.Equal(flag.Name, result.Name);
    }

    [Fact]
    public async Task Post_InvalidFlagWithExistentName_ReturnsBadRequest()
    {
        var flag = await GetFlag();
        var flagWithExistentName = new FlagFake(name: flag.Name).Generate();

        var result = await PostFromBody<AppException>(_flagClient, flagWithExistentName);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
    }

    [Fact]
    public async Task Post_InvalidFlagWithoutName_ReturnsBadRequest()
    {
        var flag = new FlagFake(name: "").Generate();

        var result = await PostFromBody<AppException>(_flagClient, flag);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
    }

    [Fact]
    public async Task Put_ValidFlag_ReturnsOkResult()
    {
        var flag = await GetFlag();
        var updatedFlag = new FlagFake(id: flag.Id).Generate();

        var result = await PutFromBody<Flag>(_flagClient, updatedFlag);

        Assert.Equal(updatedFlag.Name, result.Name);
    }

    [Fact]
    public async Task Put_InvalidFlagWithExistentName_ReturnsBadRequest()
    {
        var flag1 = await GetFlag();
        var flag2 = await GetFlag();
        var flagWithExistentName = new FlagFake(id: flag1.Id, name: flag2.Name).Generate();

        var result = await PutFromBody<AppException>(_flagClient, flagWithExistentName);

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
    }

    [Fact]
    public async Task Put_InvalidFlagWithNotExistentId_ReturnsNotFound()
    {
        var flag = new FlagFake(id: Guid.NewGuid().ToString()).Generate();

        var result = await PutFromBody<AppException>(_flagClient, flag);

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }

    [Fact]
    public async Task Delete_ValidId_ReturnsOkResult()
    {
        var flag = await GetFlag();

        var result = await DeleteFromUri<Flag>(_flagClient, flag.Id!);

        Assert.Equal(flag.Name, result.Name);
    }

    [Fact]
    public async Task Delete_InvalidId_ReturnsNotFound()
    {
        var result = await DeleteFromUri<AppException>(_flagClient, Guid.NewGuid().ToString());

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }
}
