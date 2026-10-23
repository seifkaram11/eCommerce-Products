using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Products.Core.RepositoryContrast;
using Products.Infrastructure.Data;
using Products.Infrastructure.Repository;

namespace Products.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddScoped<IProductsRepository,ProductsRepository>();
        service.AddScoped<ICategoryRepository,CategoryRepository>();
        service.AddScoped<IBrandRepository,BrandRepository>();
        service.AddDbContext<ProductDbContext>();

        service.AddMassTransit(_=>
        {
           _.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("product", includeNamespace: false));
           _.AddConsumers(typeof(ProductDbContext).Assembly);

            _.AddEntityFrameworkOutbox<ProductDbContext>(x =>
            {
                x.UseMySql();
                x.UseBusOutbox();
            });

            _.UsingRabbitMq((ctx, cof) =>
            {
                var rmqSeteings=configuration.GetSection("RabbitMq");

                cof.Host(rmqSeteings["Host"], "/", h =>
                {
                    h.Username(rmqSeteings["Username"]!);
                    h.Password(rmqSeteings["Password"]!);
                });

                cof.UseMessageRetry(r=>r.Interval(3, TimeSpan.FromSeconds(1)));
                cof.ConfigureEndpoints(ctx);
            });
        });

        return service;
    }
}
