using Hub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hub.Infra.Data.Configurations;

public class FileRoleConfiguration : IEntityTypeConfiguration<FileRole>
{
    public void Configure(EntityTypeBuilder<FileRole> builder)
    {
        builder.HasKey(fr => new { fr.FileId, fr.RoleId });

        builder.HasOne(fr => fr.File)
            .WithMany()
            .HasForeignKey(fr => fr.FileId);

        builder.HasOne(fr => fr.Role)
            .WithMany()
            .HasForeignKey(fr => fr.RoleId);
    }
}
