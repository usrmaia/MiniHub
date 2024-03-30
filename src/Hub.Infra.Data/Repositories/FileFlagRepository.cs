using Hub.Domain.Entities;
using Hub.Domain.Repositories;
using Hub.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Hub.Infra.Data.Repositories;

public class FileFlagRepository : BaseRepository<FileFlag>, IFileFlagRepository
{
    private readonly AppDbContext _context;

    public FileFlagRepository(AppDbContext context) : base(context) =>
        _context = context;

    public async Task<bool> ExistsByFileAndFlag(string fileId, string flagId) =>
        await _context.FilesFlags.AnyAsync(x => x.FileId == fileId && x.FlagId == flagId);
}
