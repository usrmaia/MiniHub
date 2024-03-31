using Hub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hub.Infra.Data.Configurations;

public class FlagConfiguration : IEntityTypeConfiguration<Flag>
{
    public void Configure(EntityTypeBuilder<Flag> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Id)
            .ValueGeneratedOnAdd();

        builder.Property(f => f.Name)
            .IsRequired()
            .IsUnicode();

        builder.Property(f => f.Description)
            .IsRequired();

        builder.Property(f => f.CreatedAt)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("datetime('now')");

        builder.Property(f => f.UpdatedAt)
            .IsRequired()
            .ValueGeneratedOnAddOrUpdate()
            .HasDefaultValueSql("datetime('now')");

        builder.Property(f => f.UserId).IsRequired();
        builder.HasOne(f => f.User)
            .WithMany()
            .HasForeignKey(f => f.UserId);
    }
}
