using Microsoft.Extensions.DependencyInjection;
using Products.Core.CachedContrast;
using Products.Redis.Cached;
using Products.Redis.Data;

namespace Products.Redis;

public static class DependencyInjection
{
    public static IServiceCollection AddRedis(this IServiceCollection service)
    {
        service.AddScoped<Client>();
        service.AddScoped<ICachedBrand, CachedBrand>();
        service.AddScoped<ICachedCategory, CachedCategory>();
        return service;
    }
}
