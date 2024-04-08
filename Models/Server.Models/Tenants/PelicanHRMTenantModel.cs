using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Models
{
    public class PelicanHRMTenantModel
    {
        public Guid Id                  { get; set; }
        public string CompanyName       { get; set; }
        public int CompanyId            { get; set; }
    }
}
