using Bogus;
using Hub.Domain.DTOs;

namespace Hub.Test.Fakers;

public class RoleFake : Faker<RoleDTO>
{
    public RoleFake(string? id = null, string? name = null)
    {
        RuleFor(x => x.Id, f => id ?? null);
        RuleFor(x => x.Name, f => name ?? f.Company.CompanyName());
    }
}
