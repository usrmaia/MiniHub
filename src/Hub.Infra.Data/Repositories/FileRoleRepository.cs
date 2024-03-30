using Hub.Domain.Entities;
using Hub.Domain.Repositories;
using Hub.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Hub.Infra.Data.Repositories;

public class FileRoleRepository : BaseRepository<FileRole>, IFileRoleRepository
{
    private readonly AppDbContext _context;

    public FileRoleRepository(AppDbContext context) : base(context) =>
        _context = context;

    public async Task<bool> ExistsByFileAndRole(string fileId, string roleId) =>
        await _context.FilesRoles.AnyAsync(x => x.FileId == fileId && x.RoleId == roleId);
}
