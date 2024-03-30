using Hub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hub.Infra.Data.Configurations;

public class DirectoryRoleConfiguration : IEntityTypeConfiguration<DirectoryRole>
{
    public void Configure(EntityTypeBuilder<DirectoryRole> builder)
    {
        builder.HasKey(dr => new { dr.DirectoryId, dr.RoleId });

        builder.HasOne(dr => dr.Directory)
            .WithMany()
            .HasForeignKey(dr => dr.DirectoryId);

        builder.HasOne(dr => dr.Role)
            .WithMany()
            .HasForeignKey(dr => dr.RoleId);
    }
}
