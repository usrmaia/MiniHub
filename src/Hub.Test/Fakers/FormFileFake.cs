using Bogus;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Hub.Test.Fakers;

public class FormFileFake : Faker<IFormFile>
{
    public FormFileFake(string? fileName = null)
    {
        var stream = new MemoryStream();
        var length = stream.Length;
        fileName = fileName ?? new Faker().System.FileName();
        var name = Path.GetFileNameWithoutExtension(fileName);
        var extension = Path.GetExtension(fileName)?.TrimStart('.');
        var contentTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/bmp", "image/tiff", "image/webp", "video/mp4" };
        var contentType = extension switch
        {
            "jpg" => contentTypes[0],
            "jpeg" => contentTypes[0],
            "png" => contentTypes[1],
            "gif" => contentTypes[2],
            "bmp" => contentTypes[3],
            "tiff" => contentTypes[4],
            "webp" => contentTypes[5],
            "mp4" => contentTypes[6],
            _ => contentTypes[0],
        };

        var formFileFake = new FormFile(stream, 0, length, name, fileName)
        {
            Headers = new HeaderDictionary(),
        };

        if (!string.IsNullOrEmpty(contentType))
            formFileFake.ContentType = contentType;

        formFileFake.ContentDisposition = $"form-data; name=\"{name}\"; filename=\"{fileName}\"";

        CustomInstantiator(f => formFileFake);
    }
}
