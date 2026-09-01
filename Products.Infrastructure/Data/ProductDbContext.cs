using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Products.Core.Entitys;

namespace Products.Infrastructure.Data;

public class ProductDbContext(IConfiguration _configuration):DbContext
{
    public DbSet<Product> Products{get;set;}
    public DbSet<Category> Categories{get;set;}


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        string connString=_configuration.GetConnectionString("MySQL");
        optionsBuilder.UseMySql(connString,
            ServerVersion.AutoDetect(connString));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductDbContext).Assembly);
    }
}
