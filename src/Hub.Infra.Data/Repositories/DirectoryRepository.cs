using Hub.Domain.Entities;
using Hub.Domain.Repositories;
using Hub.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Hub.Infra.Data.Repositories;

public class DirectoryRepository : BaseRepository<DirectoryE>, IDirectoryRepository
{
    private readonly AppDbContext _context;

    public DirectoryRepository(AppDbContext context) : base(context) =>
        _context = context;

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
