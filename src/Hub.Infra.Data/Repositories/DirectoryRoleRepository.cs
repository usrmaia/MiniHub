using Hub.Domain.Entities;
using Hub.Domain.Repositories;
using Hub.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Hub.Infra.Data.Repositories;

public class DirectoryRoleRepository : BaseRepository<DirectoryRole>, IDirectoryRoleRepository
{
    private readonly AppDbContext _context;

    public DirectoryRoleRepository(AppDbContext context) : base(context) =>
        _context = context;

    public async Task<bool> ExistsByDirectoryAndRole(string directoryId, string roleId) =>
        await _context.DirectoriesRoles.AnyAsync(dr => dr.DirectoryId == directoryId && dr.RoleId == roleId);
}
