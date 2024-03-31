using Bogus;
using Hub.Domain.Entities;

namespace Hub.Test.Fakers;

public class FlagFake : Faker<Flag>
{
    public FlagFake(string? id = null, string? name = null, string? description = null, DateTime? createdAt = null, DateTime? updatedAt = null, string? userId = null)
    {
        RuleFor(x => x.Id, f => id ?? null);
        RuleFor(x => x.Name, f => name ?? f.Random.Words());
        RuleFor(x => x.Description, f => description ?? f.Lorem.Sentence());
        RuleFor(x => x.CreatedAt, f => createdAt ?? DateTime.Now);
        RuleFor(x => x.UpdatedAt, f => updatedAt ?? DateTime.Now);
        RuleFor(x => x.UserId, f => userId ?? "");
    }
}
