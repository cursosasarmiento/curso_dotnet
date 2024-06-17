using Curso.AspIdentity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Curso.AspIdentity
{
    public static class AspIdentityServiceRegister
    {
        public static IServiceCollection RegisterAspIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("AspIdentityConnectionString");
            services.AddDbContext<AspIdentityDbContext>(options =>
            {
                options.UseSqlServer(connectionString, x => x.MigrationsAssembly(typeof(AspIdentityDbContext).Assembly.FullName));
            });
            services.AddIdentity<UsuarioIdentity, IdentityRole>().
                AddEntityFrameworkStores<AspIdentityDbContext>().
                AddDefaultTokenProviders();

            return services;
        }
    }
}
