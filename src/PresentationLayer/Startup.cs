using DataLayer;
using DataLayer.EFCore;
using DomainLayer;
using DomainLayer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PresentationLayer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataLayer();
            services.AddDomainLayer();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.SeedDataLayer();
        }
    }

    internal static class DataLayerModule
    {
        internal static IServiceCollection AddDataLayer(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddDbContext<ProductContext>(options => options
                .UseInMemoryDatabase("ProductContextMemoryDB")
                .ConfigureWarnings(builder => builder.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            );
            return services;
        }

        internal static IApplicationBuilder SeedDataLayer(this IApplicationBuilder app)
        {
            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ProductContext>();
                context.Products.Add(new() { 
                    Id = 1,
                    Name = "Apple",
                    QuantityInStock = 10
                });
                context.Products.Add(new()
                {
                    Id = 2,
                    Name = "Orange",
                    QuantityInStock = 20
                });
                context.Products.Add(new()
                {
                    Id = 3,
                    Name = "Banana",
                    QuantityInStock = 30
                });
                context.SaveChanges();
            }
            return app;
        }
    }

    internal static class DomainLayerModule
    {
        internal static IServiceCollection AddDomainLayer(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IStockService, StockService>();
            return services;
        }
    }
}
