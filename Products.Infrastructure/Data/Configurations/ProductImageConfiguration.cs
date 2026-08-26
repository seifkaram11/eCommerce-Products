using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Products.Core.Entitys;

namespace Products.Infrastructure.Data.Configurations;

class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.HasKey(_=>_.ProductImageId);
        builder.Property(_=>_.ProductImageId).HasColumnType("uniqueidentifier");
        builder.Property(_=>_.ImageUrl).HasColumnType("NVARCHAR").HasMaxLength(255);
        builder.Property(_=>_.DisplayOrder).HasColumnType("INT");
        builder.Property(_=>_.IsPrimary).HasColumnType("boolean");

        builder.HasOne(_=>_.Product).WithMany(_=>_.Images).HasForeignKey(_=>_.ProductId).OnDelete(DeleteBehavior.Cascade);

        builder.ToTable(nameof(ProductImage));
    }
}
