using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Curso.AspIdentity.Seeders
{
    public class UserRoleSeeder : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                new IdentityUserRole<string>()
                {
                    UserId = "3859b802-e72e-40fe-bccb-f67cf695e5b4",
                    RoleId = "dc2c4f8f-f2f0-46ba-b7ba-6662beaf782c"
                },
                new IdentityUserRole<string>()
                {
                    UserId = "09d50506-517b-4204-b3eb-441dfe2154f2",
                    RoleId = "c152507b-a992-4880-942a-8313447e729a"
                }
                
                );
        }
    }
}
