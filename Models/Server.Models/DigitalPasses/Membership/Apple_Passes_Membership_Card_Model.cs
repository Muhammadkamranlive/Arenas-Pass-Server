using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Models
{
    public class Apple_Passes_Membership_Card_Model
    {
        public string Logo_Text            { get; set; }
        public string Member_Name          { get; set; }
        public string Membership_Number    { get; set; }
        public string Membership_Tier      { get; set; }
        public DateTime? Expiration_Date   { get; set; }
        public string Additional_Benefits  { get; set; }
        public string Logo_Url             { get; set; }
        public string Background_Color     { get; set; }
        public string Foreground_Color     { get; set; }
        public string Label_Color          { get; set; }
        public string Organization_Name    { get; set; }
        public string Description          { get; set; }
        public string Privacy_Policy       { get; set; }
        public string Code_Type            { get; set; } 
        public string Website              { get; set; }
        public string Notes                { get; set; }
    }

}
