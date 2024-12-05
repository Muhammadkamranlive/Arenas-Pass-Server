using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{  
    public class Account_Balance
    {
        public int    Id                        { get; set; }
        public string Tenant_Id                 { get; set; }
        public string ACCOUNT_NO                { get; set; }
        public string Account_Type              { get; set; }
        public string Customer_FName            { get; set; }
        public string Customer_LName            { get; set; }
        public string Customer_Email            { get; set; }
        public string Process_By                { get; set; }
        public string Processor                 { get; set; }
        public DateTime Created_At              { get; set; }
        public string Txn_Time                  { get; set; }
        public string Account_Status            { get; set; }
        public decimal Amount                   { get; set; }

        public Account_Balance()
        {
            Created_At = DateTime.Now;
            Txn_Time   = DateTime.Now.TimeOfDay.ToString();
        }
    }
}
