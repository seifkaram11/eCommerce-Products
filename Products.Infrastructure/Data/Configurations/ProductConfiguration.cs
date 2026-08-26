using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Products.Core.Entitys;

namespace Products.Infrastructure.Data.Configurations;

class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(_=>_.ProductId);
        builder.Property(_=>_.ProductId).HasColumnType("uniqueidentifier");
        builder.Property(_=>_.Name).HasColumnType("NVARCHAR").HasMaxLength(255);
        builder.Property(_=>_.Description).HasColumnType("TEXT");
        builder.Property(_=>_.Price).HasColumnType("decimal");

        builder.HasOne(_=>_.Category).WithMany(_=>_.Products).HasForeignKey(_=>_.CategoryId).OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(_=>_.Images).WithOne(_=>_.Product).HasForeignKey(_=>_.ProductId).OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(_=>_.Variants).WithOne(_=>_.Product).HasForeignKey(_=>_.ProductId).OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(_=>_.Brand).WithMany(_=>_.Products).HasForeignKey(_=>_.BrandId).OnDelete(DeleteBehavior.Restrict);

        builder.ToTable(nameof(Product));
    }
}
