using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Models
{
    public class PaymentPlansModel
    {
        public int Id { get; set; }
        public string Plan_Name              { get; set; } 
        public string Plan_Description       { get; set; }
        public decimal Price                 { get; set; } 
        public string Currency_Code          { get; set; } 
        public string Billing_Cycle          { get; set; } 
        public int? Trial_Period_Days        { get; set; } 
        public int Max_Users                 { get; set; } 
        public int Max_Cards                 { get; set; } 
        public bool Supports_Custom_Branding { get; set; } 
        public bool Is_Active                { get; set; }

        
    }
}
