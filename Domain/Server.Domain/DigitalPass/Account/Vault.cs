using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class Vault
    {
        public int     Id                { get; set; }
        public int      TenantId         { get; set; }
        public string   UserId           { get; set; }
        public string   Email            { get; set; }
        public decimal  Amount           { get; set; }
        public string   VaultType        { get; set; }
        public DateTime Lastupdated      { get; set; }
        public Vault()
        {
            Lastupdated = DateTime.UtcNow;
        }

    }
}
