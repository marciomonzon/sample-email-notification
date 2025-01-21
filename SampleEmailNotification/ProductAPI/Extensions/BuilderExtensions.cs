using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProductAPI.Data;
using ProductAPI.Data.Repository;
using ProductAPI.Services;

namespace API
{
    public static class BuilderExtensions
    {
        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
        }

        public static void AddSwaggerDocs(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Product Service",
                    Description = "This Service is part of the Sample Email Notification Project",
                });
            });
        }

        public static void AddDatabase(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;
            builder.Services.AddDbContext<AppDbContext>(options => options
                            .UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }

        public static void AddJwtAuth(this WebApplicationBuilder builder)
        {
        }

        public static void AddScopedServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IProductService, ProductService>();

            builder.Services.AddScoped<IProductRepository, ProductRepository>();
        }

        public static void AddRabbitMq(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;

            builder.Services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration.GetValue<string>("RabbitMQ:Host"), h =>
                    {
                        h.Username(configuration.GetValue<string>("RabbitMQ:Username")!);
                        h.Password(configuration.GetValue<string>("RabbitMQ:Password")!);
                    });
                });
            });
        }
    }
}
