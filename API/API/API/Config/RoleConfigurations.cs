using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CandidateCRM.Config
{
    public class RoleConfigurations : IEntityTypeConfiguration<IdentityRole>
    {
        private readonly string[] _roles = new string[]
        {
        "Administrator",
        "Developer",
        "Student",
        "Resturant"
            // Add more roles here if needed
        };

        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            foreach (var roleName in _roles)
            {

                var role = new IdentityRole
                {

                    Name = roleName,
                    NormalizedName = roleName.ToUpper()
                };
                builder.HasData(role);
            }
        }
    }
}
