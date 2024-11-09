using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Models
{
    public class TenantUpdate
    {
        public string PhoneNumber       { get; set; }
        public string AddressLine1      { get; set; }
        public string City              { get; set; }
        public string State             { get; set; }
        public string Country           { get; set; }
        public string ZipCode           { get; set; }
        public string isMailingAddress  { get; set; }
        public string isPhysicalAddress { get; set; }
        
    }
}
