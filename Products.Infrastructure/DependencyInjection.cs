using Microsoft.Extensions.DependencyInjection;
using Products.Infrastructure.Data;

namespace Products.Infrastructure;

static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(IServiceCollection service)
    {
        service.AddDbContext<ProductDbContext>();
        return service;
    }
}
