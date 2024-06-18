using Curso.AspIdentity.Entities;
using Curso.Domain.AspIdentity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Curso.AspIdentity
{
    public static class AspIdentityServiceRegister
    {
        public static IServiceCollection RegisterAspIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(x =>
            {
                var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
                return jwtSettings;
            });

            var connectionString = configuration.GetConnectionString("AspIdentityConnectionString");
            services.AddDbContext<AspIdentityDbContext>(options =>
            {
                options.UseSqlServer(connectionString, x => x.MigrationsAssembly(typeof(AspIdentityDbContext).Assembly.FullName));
            });
            services.AddIdentity<UsuarioIdentity, IdentityRole>().
                AddEntityFrameworkStores<AspIdentityDbContext>().
                AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => 
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!))
                };
            }
            );

            return services;
        }
    }
}
