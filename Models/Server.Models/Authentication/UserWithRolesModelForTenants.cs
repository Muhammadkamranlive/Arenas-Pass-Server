using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Models
{
    public class UserWithRolesModelForTenants
    {
        public string Id                 { get; set; }
        public string Name               { get; set; }
        public string Email              { get; set; }
        public string Role               { get; set; }
        public string Image              { get; set; }
        public string CompanyName        { get; set; }
        public int TenantId              { get; set; }
        public string CompanyDesignation { get; set; }
        public IList<UserWithRolesModelForTenants> List { get; set; }
    }
}
