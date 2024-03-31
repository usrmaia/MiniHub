using Bogus;
using Hub.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Hub.Test.Fakers;

public class DirectoryFake : Faker<DirectoryE>
{
    public DirectoryFake(string? id = null, string? parentId = null, string? name = null, string? description = null, DateTime? createdAt = null, DateTime? updatedAt = null, List<FileE>? files = null, List<Flag>? flags = null, List<IdentityRole>? roles = null, string? userId = null)
    {
        RuleFor(d => d.Id, f => id ?? null);
        RuleFor(d => d.ParentId, f => parentId ?? null);
        RuleFor(d => d.Name, f => name ?? f.Random.Word());
        RuleFor(d => d.Description, f => description ?? f.Lorem.Sentence());
        RuleFor(d => d.CreatedAt, f => createdAt ?? f.Date.Past());
        RuleFor(d => d.UpdatedAt, f => updatedAt ?? f.Date.Past());
        RuleFor(d => d.Files, f => files ?? []);
        RuleFor(d => d.Flags, f => flags ?? []);
        RuleFor(d => d.Roles, f => roles ?? []);
        RuleFor(d => d.UserId, f => userId ?? "");
    }
}
