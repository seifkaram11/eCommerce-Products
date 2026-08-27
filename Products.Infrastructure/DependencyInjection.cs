using Microsoft.Extensions.DependencyInjection;
using Products.Core.DTOs;
using Products.Core.RepositoryContrast;
using Products.Infrastructure.Data;
using Products.Infrastructure.Repository;

namespace Products.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection service)
    {
        service.AddScoped<IProductsRepository,ProductsRepository>();
        service.AddDbContext<ProductDbContext>();
        return service;
    }
}
