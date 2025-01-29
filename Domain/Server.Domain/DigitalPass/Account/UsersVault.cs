using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class UsersVault
    {
        public int    Id          { get; set; }
        public int    TenantId    { get; set; }
        public string UserId     { get; set; }
        public string UserEmail  { get; set; }
        public string StoreId    { get; set; }
        public string StoreName  { get; set; }
        public string StoreEmail { get; set; }
        public decimal Amount    { get; set; }
        public string Redemption { get; set; }

    }
}
