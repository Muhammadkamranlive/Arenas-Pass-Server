using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Models
{
    public class Apple_Passes_Ticket_Model
    {
        public string Team_Identifier   { get; set; } 
        public string Serial_Number     { get; set; } 
        public string Organization_Name { get; set; } 
        public string Event_Name        { get; set; } 
        public DateTime Event_Date      { get; set; } 
        public string Seat_Number       { get; set; } 
        public string Background_Color  { get; set; } 
        public string Foreground_Color  { get; set; } 
        public string Barcode           { get; set; } 
        public string Logo_Url          { get; set; }
        public string Venue             { get; set; }
        public string Label_Color       { get; set; }
        public string Logo_Text         { get; set; }
        public string Privacy_Policy    { get; set; }
        public string Description       { get; set; }
    }

}
