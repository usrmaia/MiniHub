using Hub.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Hub.API.InputModels;

public class PostFileIM
{
    public required IFormFile FormFile { get; set; }
    public FileE? FileE { get; set; }
}
