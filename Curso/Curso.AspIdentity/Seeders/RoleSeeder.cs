using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Curso.AspIdentity.Seeders
{
    public class RoleSeeder : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole()
                {
                    Id = "dc2c4f8f-f2f0-46ba-b7ba-6662beaf782c",
                    Name = "Administrator",
                    NormalizedName = ("Administrator").ToUpper()
                },
                new IdentityRole()
                {
                    Id = "c152507b-a992-4880-942a-8313447e729a",
                    Name = "Arrendador",
                    NormalizedName = ("Arrendador").ToUpper()
                },
                new IdentityRole()
                {
                    Id = "52191c14-0d8b-4300-bacc-6afc2a65c738",
                    Name = "Arrendatario",
                    NormalizedName = ("Arrendatario").ToUpper()
                }
                );
        }
    }
}
