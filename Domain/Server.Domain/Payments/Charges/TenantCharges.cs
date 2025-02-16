using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class TenantCharges
    {
        public int        Id                { get; set; }
        public string     ChargeName        { get; set; }
        public string     ChargeType        { get; set; }  
        public decimal?   ChargeAmount      { get; set; }  
        public string     ChargeDescription { get; set; } 
        public string     Status            { get; set; }  
        public DateTime   CreatedAt         { get; set; }
        public DateTime?  UpdatedAt         { get; set; }
        public string     UserId            { get; set; }
        public TenantCharges()
        {
            CreatedAt = DateTime.UtcNow;
        }
    }

    


}
