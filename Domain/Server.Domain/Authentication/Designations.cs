using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class Designations
    {
        public Guid Id             { get; set; }
        public string Designation  { get; set; }    
        public int TenantId        { get; set; }=1;

    }
}
