using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Products.Core.Mapping;
using Products.Core.Service;
using Products.Core.ServiceContrast;
using Products.Core.Validator;

namespace Products.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { },typeof(DependencyInjection).Assembly);
        services.AddScoped<IProductService,ProductService>();
        services.AddScoped<ICategoryService,CategoryService>();
        services.AddValidatorsFromAssemblyContaining<ProductAddRequestValidator>();
        return services;
    }
}
