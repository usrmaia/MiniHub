using Hub.Domain.Entities;
using Hub.Infra.Data.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hub.Infra.Data.Context;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Flag> Flags { get; set; }
    public DbSet<FileE> Files { get; set; }
    public DbSet<FileRole> FilesRoles { get; set; }
    public DbSet<FileFlag> FilesFlags { get; set; }
    public DbSet<DirectoryE> Directories { get; set; }
    public DbSet<DirectoryRole> DirectoriesRoles { get; set; }
    public DbSet<DirectoryFlag> DirectoriesFlags { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new RoleConfiguration());
        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new UserRoleConfiguration());
        builder.ApplyConfiguration(new FlagConfiguration());
        builder.ApplyConfiguration(new FileConfiguration());
        builder.ApplyConfiguration(new FileRoleConfiguration());
        builder.ApplyConfiguration(new FileFlagConfiguration());
        builder.ApplyConfiguration(new DirectoryConfiguration());
        builder.ApplyConfiguration(new DirectoryRoleConfiguration());
        builder.ApplyConfiguration(new DirectoryFlagConfiguration());
    }
}