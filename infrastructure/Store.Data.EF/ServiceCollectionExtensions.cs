using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Store.Data.EF
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEfRepositories(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<StoreDbContext>(
                options =>
                {
                    options.UseSqlServer(connectionString);
                },
                ServiceLifetime.Transient
            );
            /*
             builder.Services.AddTransient<ICounter, RandomCounter>();
             builder.Services.AddTransient<CounterService>();
            */

            /*
             builder.Services.AddScoped<ICounter, RandomCounter>();
             builder.Services.AddScoped<CounterService>();
             */

            /*
             builder.Services.AddSingleton<ICounter, RandomCounter>();
             builder.Services.AddSingleton<CounterService>();
             */
            //На каждый запрос пользователя создаём свой словарь
            services.AddScoped<Dictionary<Type, StoreDbContext>>(); 

            services.AddScoped<DbContextFactory>(); 
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            return services;
        }
    }
}
 