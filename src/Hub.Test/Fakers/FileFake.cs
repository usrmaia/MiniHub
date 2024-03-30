using Bogus;
using Hub.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Hub.Test.Fakers;

public class FileFake : Faker<FileE>
{
    public FileFake(string? id = null, string? name = null, string? description = null, decimal? length = null, string? type = null, DateTime? createdAt = null, DateTime? updatedAt = null, string? path = null, string? directoryId = null, List<Flag>? flags = null, List<IdentityRole>? roles = null, string? userId = null, IdentityUser? user = null)
    {
        RuleFor(x => x.Id, f => id ?? null);
        RuleFor(x => x.Name, f => name ?? f.System.FileName());
        RuleFor(x => x.Description, f => description ?? f.Lorem.Sentence());
        RuleFor(x => x.Length, f => length ?? f.Random.Decimal());
        RuleFor(x => x.Type, f => type ?? f.System.MimeType());
        RuleFor(x => x.Path, f => path ?? f.System.DirectoryPath().Split("/").Last());
        RuleFor(x => x.DirectoryId, f => directoryId ?? null);
        RuleFor(x => x.Flags, f => flags ?? null);
        RuleFor(x => x.Roles, f => roles ?? null);
        RuleFor(x => x.UserId, f => userId ?? null);
    }
}
