using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Models
{
    public class Redeem_Gift_Card_Model
    {
        public int? Id                    { get; set; }
        public int Card_Id                { get; set; }
        public string Card_Type           { get; set; }
        public string Customer_First_Name { get; set; }
        public string Customer_Last_Name  { get; set; }
        public string Email               { get; set; }
        public decimal Amount             { get; set; }
        public string Redemption_Type     { get; set; }
    }

    public class Redeem_Gift_CardByUserId_Model
    {
        public int Card_Id                { get; set; }
        public decimal Amount             { get; set; }
        public string Redemption_Type     { get; set; }
        public string CustomerId          { get; set; }
    }
}
