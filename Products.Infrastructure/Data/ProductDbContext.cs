using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Products.Core.Entitys;

namespace Products.Infrastructure.Data;

class ProductDbContext(IConfiguration _configuration):DbContext
{
    DbSet<Product> Products{get;set;}


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseMySql(_configuration.GetConnectionString("MySSQL"),
            ServerVersion.AutoDetect(_configuration.GetConnectionString("MySSQL")));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductDbContext).Assembly);
    }
}
