using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Models
{
    public class Assign_Pass_Model
    {
        public int    Card_Id             { get; set; }
        public string Customer_Name       { get; set; }
        public string Customer_email      { get; set; }
        public string Phone               { get; set; }
        public string Store_Address       { get; set; }
        public string Store_Name          { get; set; }
        public string Customer_Type       { get; set; }="Guest";
    }
}
