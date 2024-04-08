using System.Numerics;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string  FirstName            { get; set; }
        public string? MiddleName           { get; set; }
        public string  LastName             { get; set; }
        public string? image                { get; set; }
        public bool    isAdmin              { get; set; } = false;
        public bool    isEmployee           { get; set; } = false;
        public string? defaultPassword      { get; set; }
        public int     EmployeeId           { get; set; }
        public string  CompanyName          { get; set; }="";
        public int     TenantId             { get; set; }=1;
        public string CompanyDesignation    { get; set; } = "";

    }

    public class CustomRole : IdentityRole
    {
        public string Permissions { get; set; }
    }


    public class CustomUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, CustomRole>
    {
        public CustomUserClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            RoleManager<CustomRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            // Add custom claims, such as permissions
            if (UserManager != null)
            {
                var roles = await UserManager.GetRolesAsync(user);

                foreach (var role in roles)
                {
                    var customRole = await RoleManager.FindByNameAsync(role);

                    if (customRole != null && !string.IsNullOrEmpty(customRole.Permissions))
                    {
                        identity.AddClaim(new Claim("Permission", customRole.Permissions));
                    }
                }
            }

            return identity;
        }
    }


}