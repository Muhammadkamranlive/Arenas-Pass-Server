using System;
using System.Linq;
using System.Text;
using Server.Configurations;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class Account_Transaction
    {
        public int Id                     { get; set; }
        public int Card_Id                { get; set; }
        public string Card_Type           { get; set; }
        public string Customer_First_Name { get; set; }
        public string Customer_Last_Name  { get; set; } = "";
        public string Email               { get; set; }
        public decimal Amount             { get; set; }
        public string DrCrFlag            { get; set; }
        public string Txn_Time            { get; set; }
        public string Processor_Id        { get; set; }
        public string Processor_Name      { get; set; }
        public string Tenant_Id           { get; set; }
        public DateTime Created_At        { get; set; }
        public string Txn_Type            { get; set; }
        public string RedemptionType      { get; set; }=RedemptionStatusModel.InStore;

        public Account_Transaction()
        {
            Created_At = DateTime.Now;
            Txn_Time   = DateTime.Now.TimeOfDay.ToString();
        }
    }
}
