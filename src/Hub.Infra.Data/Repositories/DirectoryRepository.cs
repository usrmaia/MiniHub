using Hub.Domain.Entities;
using Hub.Domain.Filters;
using Hub.Domain.Repositories;
using Hub.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Hub.Infra.Data.Repositories;

public class DirectoryRepository : BaseRepository<DirectoryE>, IDirectoryRepository
{
    private readonly AppDbContext _context;

    public DirectoryRepository(AppDbContext context) : base(context) =>
        _context = context;

    public async Task<QueryResult<DirectoryE>> Query(DirectoryFilter filter)
    {
        var query = _context.Directories.AsNoTracking();

        if (!string.IsNullOrEmpty(filter.Id)) query = query.Where(d => d.Id == filter.Id);
        if (!string.IsNullOrEmpty(filter.Name)) query = query.Where(d => d.Name.ToUpper().Contains(filter.Name.ToUpper()));
        if (!string.IsNullOrEmpty(filter.UserId)) query = query.Where(d => d.UserId == filter.UserId);
        if (!string.IsNullOrEmpty(filter.Role)) query = query.Where(d => _context.Roles.Any(r => r.Name!.ToUpper().Contains(filter.Role.ToUpper()) &&
            _context.DirectoriesRoles.Any(dr => dr.RoleId == r.Id && dr.DirectoryId == d.Id)));
        if (!string.IsNullOrEmpty(filter.Flag)) query = query.Where(d => _context.Flags.Any(f => f.Name!.ToUpper().Contains(filter.Flag.ToUpper()) &&
            _context.DirectoriesFlags.Any(df => df.FlagId == f.Id && df.DirectoryId == d.Id)));

        if (!string.IsNullOrEmpty(filter.NameOrderSort)) query = filter.NameOrderSort.ToLower() == "asc" ? query.OrderBy(d => d.Name) : query.OrderByDescending(d => d.Name);
        if (!string.IsNullOrEmpty(filter.CreatedAtOrderSort)) query = filter.CreatedAtOrderSort.ToLower() == "asc" ? query.OrderBy(d => d.CreatedAt) : query.OrderByDescending(d => d.CreatedAt);
        if (!string.IsNullOrEmpty(filter.UpdatedAtOrderSort)) query = filter.UpdatedAtOrderSort.ToLower() == "asc" ? query.OrderBy(d => d.UpdatedAt) : query.OrderByDescending(d => d.UpdatedAt);

        var count = await query.CountAsync();

        var directories = await query
            .Skip(filter.PageIndex * filter.PageSize)
            .Take(filter.PageSize)
            .Include(d => d.User)
            .Select(d => new DirectoryE
            {
                Id = d.Id,
                Name = d.Name,
                UserId = d.UserId,
                User = d.User,
                ParentId = d.ParentId,
                CreatedAt = d.CreatedAt,
                UpdatedAt = d.UpdatedAt,
                Flags = _context.DirectoriesFlags
                    .Where(df => df.DirectoryId == d.Id)
                    .Join(_context.Flags, df => df.FlagId, f => f.Id, (df, f) => f)
                    .ToList(),
                Roles = _context.DirectoriesRoles
                    .Where(dr => dr.DirectoryId == d.Id)
                    .Join(_context.Roles, dr => dr.RoleId, r => r.Id, (dr, r) => r)
                    .ToList()
            })
            .ToListAsync();

        return new(directories, count);
    }

    public async Task<bool> ExistsByNameAndParentId(string name, string? parentId) =>
        await _context.Directories.AnyAsync(d => d.Name == name && d.ParentId == parentId);

    public Task<List<string>> GetChildrensNameById(string id) =>
        _context.Directories
            .Where(d => d.ParentId == id)
            .Select(d => d.Name)
            .ToListAsync();

    public async Task<List<string>> GetParentsNameById(string id)
    {
        var parent = await GetById(id);
        var parents = new List<string>() { parent.Name };

        if (string.IsNullOrEmpty(parent.ParentId))
            return parents;

        while (!string.IsNullOrEmpty(parent.ParentId))
        {
            parent = await GetById(parent.ParentId!);
            parents.Add(parent.Name);
        }

        return parents.Reverse<string>().ToList();
    }
}
