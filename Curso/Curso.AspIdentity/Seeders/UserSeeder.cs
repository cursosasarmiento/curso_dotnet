using Curso.AspIdentity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Curso.AspIdentity.Seeders
{
    public class UserSeeder : IEntityTypeConfiguration<UsuarioIdentity>
    {
        public void Configure(EntityTypeBuilder<UsuarioIdentity> builder)
        {
            var hasher = new PasswordHasher<UsuarioIdentity>();

            builder.HasData(
                new UsuarioIdentity()
                {
                    Id = "3859b802-e72e-40fe-bccb-f67cf695e5b4",
                    Email = "Administrator@curso.com",
                    NormalizedEmail = ("Administrator@curso.com").ToUpper(),
                    EmailConfirmed = true,
                    Nombre = "Administrator",
                    Apellidos = "App",
                    UserName = "Administrator",
                    NormalizedUserName = ("Administrator").ToUpper(),
                    PasswordHash = hasher.HashPassword(null, "Password?2024")
                },
                new UsuarioIdentity()
                {
                    Id = "09d50506-517b-4204-b3eb-441dfe2154f2",
                    Email = "Arrendador@curso.com",
                    NormalizedEmail = ("Arrendador@curso.com").ToUpper(),
                    EmailConfirmed = true,
                    Nombre = "Arrendador",
                    Apellidos = "App",
                    UserName = "Arrendador",
                    NormalizedUserName = ("Arrendador").ToUpper(),
                    PasswordHash = hasher.HashPassword(null, "Password?2024")
                }


                );
        }
    }
}
