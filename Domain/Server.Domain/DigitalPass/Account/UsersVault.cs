using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class UsersVault
    {
        public int     Id                { get; set; }
        public int      TenantId         { get; set; }
        public string   UserId           { get; set; }
        public string   UserEmail        { get; set; }
        public decimal  Amount           { get; set; }
        public string   VaultType        { get; set; }
        public DateTime Lastupdated      { get; set; }
        public UsersVault()
        {
            Lastupdated = DateTime.UtcNow;
        }

    }
}
