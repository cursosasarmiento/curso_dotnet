using Curso.AspIdentity.Entities;
using Curso.AspIdentity.Seeders;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Curso.AspIdentity
{
    public class AspIdentityDbContext: IdentityDbContext<UsuarioIdentity>
    {
        public AspIdentityDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new RoleSeeder());
            builder.ApplyConfiguration(new UserSeeder());
            builder.ApplyConfiguration(new UserRoleSeeder());
        }
    }
}
