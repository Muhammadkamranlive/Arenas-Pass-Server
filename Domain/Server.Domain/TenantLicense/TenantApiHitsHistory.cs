using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class TenantApiHitsHistory
    {
        public int    Id          { get; set; }
        public string privateKey  { get; set; }
        public string publicKey   { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TenantId       { get; set; }
        public string ApiMethod   { get; set; } ="";
        public string RequestType { get; set; } = "Get";

        public TenantApiHitsHistory()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
