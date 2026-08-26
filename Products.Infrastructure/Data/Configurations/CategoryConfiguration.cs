using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Products.Core.Entitys;

namespace Products.Infrastructure.Data.Configurations;

class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(_=>_.CategoryId);
        builder.Property(_=>_.CategoryId).HasColumnType("uniqueidentifier");
        builder.Property(_=>_.Name).HasColumnType("NVARCHAR").HasMaxLength(255);
        builder.Property(_=>_.Description).HasColumnType("TEXT");

        builder.HasOne(c => c.ParentCategory).WithMany(c => c.SubCategories).HasForeignKey(c => c.ParentCategoryId).OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(_=>_.Products).WithOne(_=>_.Category).HasForeignKey(_=>_.CategoryId).OnDelete(DeleteBehavior.Restrict);

        builder.ToTable(nameof(Category));
    }
}
