using Hub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hub.Infra.Data.Configurations;

public class FileFlagConfiguration : IEntityTypeConfiguration<FileFlag>
{
    public void Configure(EntityTypeBuilder<FileFlag> builder)
    {
        builder.HasKey(ff => new { ff.FileId, ff.FlagId });

        builder.HasOne(ff => ff.File)
            .WithMany()
            .HasForeignKey(ff => ff.FileId);

        builder.HasOne(ff => ff.Flag)
            .WithMany()
            .HasForeignKey(ff => ff.FlagId);
    }
}
