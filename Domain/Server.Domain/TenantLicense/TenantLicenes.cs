using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class TenantLicenes
    {
        public int    Id          { get; set; }
        public string privateKey  { get; set; }
        public string publicKey   { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime Update_At { get; set; }
        public int TenantId       { get; set; }
        public string Status      { get; set; }
        public TenantLicenes()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
