using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Products.Core.Entitys;

namespace Products.Infrastructure.Data.Configurations;

class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.HasKey(_=>_.ProductVariantId);
        builder.Property(_=>_.ProductVariantId).HasColumnType("uniqueidentifier");
        builder.Property(_=>_.SKU).HasColumnType("NVARCHAR").HasMaxLength(255);
        builder.Property(_=>_.Price).HasColumnType("decimal");

        builder.HasOne(_=>_.Product).WithMany(_=>_.Variants).HasForeignKey(_=>_.ProductId).OnDelete(DeleteBehavior.Cascade);

        builder.ToTable(nameof(ProductVariant));
    }
}
