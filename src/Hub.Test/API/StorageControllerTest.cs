using System.Net;
using Bogus;
using Hub.API.InputModels;
using Hub.Domain.DTOs;
using Hub.Domain.Entities;
using Hub.Domain.Exceptions;
using Hub.Test.DTOs;
using Hub.Test.Fakers;
using Hub.Test.Global;
using Microsoft.AspNetCore.Http;

namespace Hub.Test.API;

public class StorageControllerTest : GlobalClientRequest
{
    [Fact]
    public async Task Post_ValidUpload_ReturnsOk()
    {
        var formFile = new FormFileFake().Generate();
        var file = new FileFake(name: formFile.FileName, type: formFile.ContentType, length: formFile.Length).Generate();
        var model = new PostFileIM { FormFile = formFile, FileE = file };

        var result = await PostFromForm<FileE>(_uploadStorageClient, model);

        Assert.Equal(file.Name, result.Name);
        Assert.Equal(file.Length, result.Length);
    }

    [Fact]
    public async Task Post_ValidUploadWithParent_ReturnsOk()
    {
        var parent = await GetDirectory();
        var formFile = new FormFileFake().Generate();
        var file = new FileFake(name: formFile.FileName, type: formFile.ContentType, length: formFile.Length, directoryId: parent.Id).Generate();
        var model = new PostFileIM { FormFile = formFile, FileE = file };

        var result = await PostFromForm<FileE>(_uploadStorageClient, model);

        Assert.Equal(file.Name, result.Name);
        Assert.Equal(file.Length, result.Length);
        Assert.Equal(parent.Id, result.DirectoryId);
    }

    [Fact]
    public async Task Get_Download_ValidDownload_ReturnsOk()
    {
        var file = await GetFile();

        var response = await GetFromUri<AppHttpResponse>(_downloadStorageClient, file.Id!);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_Download_InvalidDownload_ReturnsNotFound()
    {
        var response = await GetFromUri<AppHttpResponse>(_downloadStorageClient, Guid.NewGuid().ToString());

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Post_Move_ValidMove_ReturnsOk()
    {
        var user = await GetUser();
        var file = await GetFile(userId: user.Id);
        var directory = await GetDirectory(userId: user.Id);
        var moveDTO = new MoveDTO { FileId = file.Id!, DirectoryId = directory.Id! };

        var response = await PostFromBody<AppHttpResponse>(_moveStorageClient, moveDTO);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }


    [Fact]
    public async Task Delete_ValidDelete_ReturnsOk()
    {
        var file = await GetFile();

        var response = await DeleteFromUri<AppHttpResponse>(_storageClient, file.Id!);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Delete_InvalidDelete_ReturnsNotFound()
    {
        var response = await DeleteFromUri<AppHttpResponse>(_storageClient, Guid.NewGuid().ToString());

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Post_AddFlag_ValidFileFlag_ReturnsOk()
    {
        var file = await GetFile();
        var flag = await GetFlag();
        var fileFlag = new FileFlag { FileId = file.Id!, FlagId = flag.Id! };

        var response = await PostFromBody<FileFlag>(_addFileToFlagClient, fileFlag);

        Assert.Equal(fileFlag.FileId, response.FileId);
        Assert.Equal(fileFlag.FlagId, response.FlagId);
    }

    [Fact]
    public async Task Post_AddFlag_InvalidFlag_ReturnsNotFound()
    {
        var file = await GetFile();
        var fileFlag = new FileFlag { FileId = file.Id!, FlagId = Guid.NewGuid().ToString() };

        var response = await PostFromBody<AppException>(_addFileToFlagClient, fileFlag);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Post_AddFlag_InvalidFile_ReturnsNotFound()
    {
        var flag = await GetFlag();
        var fileFlag = new FileFlag { FileId = Guid.NewGuid().ToString(), FlagId = flag.Id! };

        var response = await PostFromBody<AppException>(_addFileToFlagClient, fileFlag);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Post_AddFlag_InvalidFileFlag_ReturnsNotFound()
    {
        var fileFlag = new FileFlag { FileId = Guid.NewGuid().ToString(), FlagId = Guid.NewGuid().ToString() };

        var response = await PostFromBody<AppException>(_addFileToFlagClient, fileFlag);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Post_AddFlag_FileFlagAlreadyExists_ReturnsConflict()
    {
        var fileFlag = await GetFileFlag();

        var response = await PostFromBody<AppException>(_addFileToFlagClient, fileFlag);

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task Delete_RemoveFlag_ValidFileFlag_ReturnsOk()
    {
        var fileFlag = await GetFileFlag();

        var response = await DeleteFromBody<FileFlag>(_removeFileFromFlagClient, fileFlag);

        Assert.Equal(fileFlag.FileId, response.FileId);
        Assert.Equal(fileFlag.FlagId, response.FlagId);
    }

    [Fact]
    public async Task Delete_RemoveFlag_InvalidFile_ReturnsNotFound()
    {
        var flag = await GetFlag();
        var fileFlag = new FileFlag { FileId = Guid.NewGuid().ToString(), FlagId = flag.Id! };

        var response = await DeleteFromBody<AppException>(_removeFileFromFlagClient, fileFlag);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_RemoveFlag_InvalidFlag_ReturnsNotFound()
    {
        var file = await GetFile();
        var fileFlag = new FileFlag { FileId = file.Id!, FlagId = Guid.NewGuid().ToString() };

        var response = await DeleteFromBody<AppException>(_removeFileFromFlagClient, fileFlag);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_RemoveFlag_InvalidFileFlag_ReturnsNotFound()
    {
        var fileFlag = new FileFlag { FileId = Guid.NewGuid().ToString(), FlagId = Guid.NewGuid().ToString() };

        var response = await DeleteFromBody<AppException>(_removeFileFromFlagClient, fileFlag);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Post_AddRole_ValidFileRole_ReturnsOk()
    {
        var file = await GetFile();
        var role = await GetRole();
        var fileRole = new FileRole { FileId = file.Id!, RoleId = role.Id! };

        var response = await PostFromBody<FileRole>(_addFileToRoleClient, fileRole);

        Assert.Equal(fileRole.FileId, response.FileId);
        Assert.Equal(fileRole.RoleId, response.RoleId);
    }

    [Fact]
    public async Task Post_AddRole_InvalidRole_ReturnsNotFound()
    {
        var file = await GetFile();
        var fileRole = new FileRole { FileId = file.Id!, RoleId = Guid.NewGuid().ToString() };

        var response = await PostFromBody<AppException>(_addFileToRoleClient, fileRole);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Post_AddRole_InvalidFile_ReturnsNotFound()
    {
        var role = await GetRole();
        var fileRole = new FileRole { FileId = Guid.NewGuid().ToString(), RoleId = role.Id! };

        var response = await PostFromBody<AppException>(_addFileToRoleClient, fileRole);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Post_AddRole_InvalidFileRole_ReturnsNotFound()
    {
        var fileRole = new FileRole { FileId = Guid.NewGuid().ToString(), RoleId = Guid.NewGuid().ToString() };

        var response = await PostFromBody<AppException>(_addFileToRoleClient, fileRole);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_RemoveRole_ValidFileRole_ReturnsOk()
    {
        var fileRole = await GetFileRole();

        var response = await DeleteFromBody<FileRole>(_removeFileFromRoleClient, fileRole);

        Assert.Equal(fileRole.FileId, response.FileId);
        Assert.Equal(fileRole.RoleId, response.RoleId);
    }

    [Fact]
    public async Task Delete_RemoveRole_InvalidFile_ReturnsNotFound()
    {
        var role = await GetRole();
        var fileRole = new FileRole { FileId = Guid.NewGuid().ToString(), RoleId = role.Id! };

        var response = await DeleteFromBody<AppException>(_removeFileFromRoleClient, fileRole);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_RemoveRole_InvalidRole_ReturnsNotFound()
    {
        var file = await GetFile();
        var fileRole = new FileRole { FileId = file.Id!, RoleId = Guid.NewGuid().ToString() };

        var response = await DeleteFromBody<AppException>(_removeFileFromRoleClient, fileRole);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_RemoveRole_InvalidFileRole_ReturnsNotFound()
    {
        var fileRole = new FileRole { FileId = Guid.NewGuid().ToString(), RoleId = Guid.NewGuid().ToString() };

        var response = await DeleteFromBody<AppException>(_removeFileFromRoleClient, fileRole);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
