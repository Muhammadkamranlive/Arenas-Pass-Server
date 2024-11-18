using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Models
{
    public class Apple_Passes_Voucher_Model
    {
        public string Team_Identifier   { get; set; }
        public string Serial_Number     { get; set; }
        public string Organization_Name { get; set; }
        public string Description       { get; set; }
        public string Amount            { get; set; }
        public DateTime Expiry_Date     { get; set; }
        public string Background_Color  { get; set; }
        public string Label_Color       { get; set; }
        public string Foreground_Color  { get; set; }
    }

}
