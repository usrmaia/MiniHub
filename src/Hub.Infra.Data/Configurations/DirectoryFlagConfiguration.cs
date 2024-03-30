using Hub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hub.Infra.Data.Configurations;

public class DirectoryFlagConfiguration : IEntityTypeConfiguration<DirectoryFlag>
{
    public void Configure(EntityTypeBuilder<DirectoryFlag> builder)
    {
        builder.HasKey(df => new { df.DirectoryId, df.FlagId });

        builder.HasOne(df => df.Directory)
            .WithMany()
            .HasForeignKey(df => df.DirectoryId);

        builder.HasOne(df => df.Flag)
            .WithMany()
            .HasForeignKey(df => df.FlagId);
    }
}
