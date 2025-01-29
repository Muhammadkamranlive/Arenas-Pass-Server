using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{   
    public class PaymentPlan
    {
        public int Id                      { get; set; }
        public string PlanName             { get; set; } 
        public string PlanDescription      { get; set; }
        public string PlanFeatures         { get; set; }
        public decimal Price               { get; set; } 
        public string Currency             { get; set; } = "USD"; 
        public string BillingCycle         { get; set; } 
        public int? TrialPeriodDays        { get; set; } 
        public int MaxUsers                { get; set; } 
        public int MaxCards                { get; set; } 
        public bool SupportsCustomBranding { get; set; } 
        public bool IsActive               { get; set; } = true;
        public DateTime CreatedAt          { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt         { get; set; }
    }
}
