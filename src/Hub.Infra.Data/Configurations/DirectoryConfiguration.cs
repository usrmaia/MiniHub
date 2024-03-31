using Hub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hub.Infra.Data.Configurations;

public class DirectoryConfiguration : IEntityTypeConfiguration<DirectoryE>
{
    public void Configure(EntityTypeBuilder<DirectoryE> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(f => f.Id)
            .ValueGeneratedOnAdd();

        builder.Property(d => d.Name).IsRequired();
        builder.Property(d => d.Description).IsRequired();

        builder.Property(d => d.ParentId)
            .IsRequired(false)
            .HasDefaultValue(null);

        builder.HasOne(d => d.Parent)
            .WithMany()
            .HasForeignKey(d => d.ParentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(f => f.CreatedAt)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("datetime('now')");

        builder.Property(f => f.UpdatedAt)
            .IsRequired()
            .ValueGeneratedOnAddOrUpdate()
            .HasDefaultValueSql("datetime('now')");

        builder.Ignore(f => f.Flags);
        builder.Ignore(f => f.Roles);

        builder.HasMany(d => d.Files)
            .WithOne(f => f.Directory)
            .HasForeignKey(f => f.DirectoryId);

        builder.Property(d => d.UserId).IsRequired();
        builder.HasOne(d => d.User)
            .WithMany()
            .HasForeignKey(d => d.UserId);
    }
}
