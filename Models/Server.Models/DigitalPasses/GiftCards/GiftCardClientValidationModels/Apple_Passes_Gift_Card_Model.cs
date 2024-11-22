using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class Apple_Passes_Gift_Card_Model
    {
        
        public string? Description          { get; set; }
        public string Logo_Text             { get; set; }
        public string Background_Color      { get; set; }
        public string Label_Color           { get; set; }
        public string Foreground_Color      { get; set; }
        public string Balance               { get; set; }
        public DateTime? Expiry_Date        { get; set; }
        public string? Card_holder_Name     { get; set; }
        public string? Card_Holder_Title    { get; set; }
        public string? Email                { get; set; }
        public string? Phone                { get; set; }
        public string? Logo_Url             { get; set; }
        public string? Privacy_Policy       { get; set; }
        public string? Code_Type            { get; set; }
        public string? Address              { get; set; }
        public string? Notes                { get; set; }
        public string? Webiste              { get; set; }
        public string Currency_Code        { get; set; }
        public string? Sender_Name          { get; set; }
    }
}
