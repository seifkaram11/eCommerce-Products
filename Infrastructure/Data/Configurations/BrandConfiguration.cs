using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Products.Core.Entitys;

namespace Products.Infrastructure.Data.Configurations;

class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.HasKey(_=>_.BrandId);
        builder.Property(_=>_.BrandId).HasColumnType("uniqueidentifier");
        builder.Property(_=>_.Name).HasColumnType("NVARCHAR").HasMaxLength(255);
        builder.Property(_=>_.Description).HasColumnType("TEXT");
        builder.Property(_=>_.LogoUrl).HasColumnType("NVARCHAR").HasMaxLength(255);

        builder.HasMany(_=>_.Products).WithOne(_=>_.Brand).HasForeignKey(_=>_.BrandId);

        builder.ToTable(nameof(Brand));
    }
}
