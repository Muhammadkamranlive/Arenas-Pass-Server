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
        public int        TenantId          { get; set; }  
        public string     CharName          { get; set; }
        public ChargeType ChargeType        { get; set; }  
        public decimal?   ChargeAmount      { get; set; }  
        public decimal?   ChargePercentage  { get; set; }  
        public decimal?   MaxChargeAmount   { get; set; }  
        public string     Currency          { get; set; }  
        public string     ChargeDescription { get; set; } 
        public string     Status            { get; set; }  
        public bool       IsDeleted         { get; set; }  
        public DateTime   CreatedAt         { get; set; }
        public DateTime?  UpdatedAt         { get; set; }
        public string     UserId            { get; set; }
        public TenantCharges()
        {
            CreatedAt = DateTime.UtcNow;
        }
    }

    public enum ChargeType
    {
        FixedAmount,     
        Percentage,      
        Hybrid           
    }


}
