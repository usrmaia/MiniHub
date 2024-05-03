using Hub.Domain.DTOs;
using Hub.Domain.Entities;
using Hub.Domain.Filters;
using Hub.Domain.Repositories;
using Hub.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Hub.Infra.Data.Repositories;

public class HubRepository : IHubRepository
{
    private readonly AppDbContext _context;

    public HubRepository(AppDbContext context) =>
        _context = context;

    public Task<Items> Query(ItemFilter filter)
    {
        var queryDirectories = _context.Directories.AsNoTracking();
        var queryFiles = _context.Files.AsNoTracking();

        if (!string.IsNullOrEmpty(filter.Name))
        {
            queryDirectories = queryDirectories.Where(d => d.Name.ToUpper().Contains(filter.Name.ToUpper()));
            queryFiles = queryFiles.Where(f => f.Name.ToUpper().Contains(filter.Name.ToUpper()));
        }
        if (!string.IsNullOrEmpty(filter.UserId))
        {
            queryDirectories = queryDirectories.Where(d => d.UserId == filter.UserId);
            queryFiles = queryFiles.Where(f => f.UserId == filter.UserId);
        }
        if (!string.IsNullOrEmpty(filter.FileId))
        {
            queryFiles = queryFiles.Where(f => f.Id == filter.FileId);
        }
        if (!string.IsNullOrEmpty(filter.DirectoryId))
        {
            queryDirectories = queryDirectories.Where(d => d.Id == filter.DirectoryId);
        }

        queryDirectories = queryDirectories.Where(d => d.ParentId == filter.ParentId);
        queryFiles = queryFiles.Where(f => f.DirectoryId == filter.ParentId);

        if (!string.IsNullOrEmpty(filter.FlagId))
        {
            queryDirectories = queryDirectories.Where(d => _context.DirectoriesFlags.Any(df => df.FlagId == filter.FlagId && df.DirectoryId == d.Id));
            queryFiles = queryFiles.Where(f => _context.FilesFlags.Any(ff => ff.FlagId == filter.FlagId && ff.FileId == f.Id));
        }
        if (!string.IsNullOrEmpty(filter.RoleId))
        {
            queryDirectories = queryDirectories.Where(d => _context.DirectoriesRoles.Any(dr => dr.RoleId == filter.RoleId && dr.DirectoryId == d.Id));
            queryFiles = queryFiles.Where(f => _context.FilesRoles.Any(fr => fr.RoleId == filter.RoleId && fr.FileId == f.Id));
        }

        if (!string.IsNullOrEmpty(filter.NameOrderSort))
        {
            queryDirectories = filter.NameOrderSort.ToLower() == "asc" ? queryDirectories.OrderBy(d => d.Name) : queryDirectories.OrderByDescending(d => d.Name);
            queryFiles = filter.NameOrderSort.ToLower() == "asc" ? queryFiles.OrderBy(f => f.Name) : queryFiles.OrderByDescending(f => f.Name);
        }
        if (!string.IsNullOrEmpty(filter.CreatedAtOrderSort))
        {
            queryDirectories = filter.CreatedAtOrderSort.ToLower() == "asc" ? queryDirectories.OrderBy(d => d.CreatedAt) : queryDirectories.OrderByDescending(d => d.CreatedAt);
            queryFiles = filter.CreatedAtOrderSort.ToLower() == "asc" ? queryFiles.OrderBy(f => f.CreatedAt) : queryFiles.OrderByDescending(f => f.CreatedAt);
        }
        if (!string.IsNullOrEmpty(filter.FlagOrderSort))
        {
            queryDirectories = filter.FlagOrderSort.ToLower() == "asc" ? queryDirectories.OrderBy(d => d.Flags) : queryDirectories.OrderByDescending(d => d.Flags);
            queryFiles = filter.FlagOrderSort.ToLower() == "asc" ? queryFiles.OrderBy(f => f.Flags) : queryFiles.OrderByDescending(f => f.Flags);
        }
        if (!string.IsNullOrEmpty(filter.UpdatedAtOrderSort))
        {
            queryDirectories = filter.UpdatedAtOrderSort.ToLower() == "asc" ? queryDirectories.OrderBy(d => d.UpdatedAt) : queryDirectories.OrderByDescending(d => d.UpdatedAt);
            queryFiles = filter.UpdatedAtOrderSort.ToLower() == "asc" ? queryFiles.OrderBy(f => f.UpdatedAt) : queryFiles.OrderByDescending(f => f.UpdatedAt);
        }
        if (!string.IsNullOrEmpty(filter.UserNameOrderSort))
        {
            queryDirectories = filter.UserNameOrderSort.ToLower() == "asc" ? queryDirectories.OrderBy(d => d.User!.UserName) : queryDirectories.OrderByDescending(d => d.User!.UserName);
            queryFiles = filter.UserNameOrderSort.ToLower() == "asc" ? queryFiles.OrderBy(f => f.User!.UserName) : queryFiles.OrderByDescending(f => f.User!.UserName);
        }

        var directories = queryDirectories
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
            });

        var files = queryFiles
            .Include(f => f.User)
            .Select(f => new FileE
            {
                Id = f.Id,
                Name = f.Name,
                UserId = f.UserId,
                User = f.User,
                DirectoryId = f.DirectoryId,
                CreatedAt = f.CreatedAt,
                UpdatedAt = f.UpdatedAt,
                Flags = _context.FilesFlags
                    .Where(ff => ff.FileId == f.Id)
                    .Join(_context.Flags, ff => ff.FlagId, f => f.Id, (ff, f) => f)
                    .ToList(),
                Roles = _context.FilesRoles
                    .Where(fr => fr.FileId == f.Id)
                    .Join(_context.Roles, fr => fr.RoleId, r => r.Id, (fr, r) => r)
                    .ToList()
            });

        var items = new Items(directories.ToList(), files.ToList(), directories.Count() + files.Count());

        return Task.FromResult(items);
    }
}
