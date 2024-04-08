using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Models.Authentication
{
    public class TenantRegisterModel
    {
        public string FirstName   { get; set; }
        public string LastName    { get; set; }
        public string? MiddleName { get; set; }
        public string Email       { get; set; }
        public string Password    { get; set; }
        public string CompanyName { get; set; }

    }
}
