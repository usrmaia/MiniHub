using Hub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hub.Infra.Data.Configurations;

public class FileConfiguration : IEntityTypeConfiguration<FileE>
{
    public void Configure(EntityTypeBuilder<FileE> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Id)
            .ValueGeneratedOnAdd();

        builder.Property(f => f.Name).IsRequired();
        builder.Property(f => f.Description).IsRequired();
        builder.Property(f => f.Length).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(f => f.Path).IsRequired();
        builder.Property(f => f.Type).IsRequired();

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

        builder.Property(f => f.DirectoryId).IsRequired(false);
        builder.HasOne(f => f.Directory)
            .WithMany(d => d.Files)
            .HasForeignKey(f => f.DirectoryId);

        builder.Property(f => f.UserId).IsRequired();
        builder.HasOne(f => f.User)
            .WithMany()
            .HasForeignKey(f => f.UserId);
    }
}