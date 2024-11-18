using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Models
{
    public class Apple_Passes_Punch_Card_Model
    {
        public string Team_Identifier        { get; set; } // Required
        public string Serial_Number          { get; set; } // Required
        public string Organization_Name      { get; set; } // Required
        public string Logo_Text              { get; set; } // Optional
        public string Background_Color       { get; set; } // Optional
        public string Foreground_Color       { get; set; } // Optional
        public string Label_Color            { get; set; } // Optional
        public string Punch_Title            { get; set; } // Required, title for the punch card (e.g., "Coffee Card")
        public int Total_Punches             { get; set; } // Required, total punches needed to complete the card
        public int Current_Punches           { get; set; } // Required, punches completed so far
        public string Reward_Details         { get; set; } // Optional, reward description upon completion
        public DateTime? Expiry_Date         { get; set; } // Optional, expiry date for the card
        public string Barcode                { get; set; } // Required for QR/Code128 representation
        public string Logo_Url               { get; set; } // Optional, URL for logo image
    }

}
