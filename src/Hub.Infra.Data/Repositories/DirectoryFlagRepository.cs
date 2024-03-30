using Hub.Domain.Entities;
using Hub.Domain.Repositories;
using Hub.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Hub.Infra.Data.Repositories;

public class DirectoryFlagRepository : BaseRepository<DirectoryFlag>, IDirectoryFlagRepository
{
    private readonly AppDbContext _context;

    public DirectoryFlagRepository(AppDbContext context) : base(context) =>
        _context = context;

    public async Task<bool> ExistsByDirectoryAndFlag(string directoryId, string flagId) =>
        await _context.DirectoriesFlags.AnyAsync(df => df.DirectoryId == directoryId && df.FlagId == flagId);
}
