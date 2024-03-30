using Hub.Domain.Entities;
using Hub.Domain.Repositories;
using Hub.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Hub.Infra.Data.Repositories;

public class FlagRepository : BaseRepository<Flag>, IFlagRepository
{
    private readonly AppDbContext _context;

    public FlagRepository(AppDbContext context) : base(context) =>
        _context = context;

    public async Task<bool> ExistByName(string name) =>
        await _context.Flags.AnyAsync(f => f.Name == name);
}
