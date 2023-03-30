using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TrainLists.Infrastructure.Extensions
{
    public static class DbContextDependencyInjection
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => 
            { 
                options.UseSqlServer(configuration.GetConnectionString(nameof(ApplicationDbContext))
                    ,provider => provider.EnableRetryOnFailure());
                options.EnableDetailedErrors(true);
            });

            return services;
        }

        public static IServiceProvider InitDbContext(this IServiceProvider provider)
        {
            using (var serviceScope = provider.CreateScope())
            {

                var ctx = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                ctx.Database.EnsureCreated();
            }
            return provider;
        }
    }
}