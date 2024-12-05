using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Models
{
    public class WalletPassModel
    {
        public int Id                                  { get; set; }
        public string Background_Color                 { get; set; }
        public string Foreground_Color                 { get; set; }
        public string Label_Color                      { get; set; }

        //Valid dates
        public DateTime? Expiration_Date               { get; set; }
        public DateTime? Relevant_Date                 { get; set; }
        public DateTime? Effective_Date                { get; set; }

        //Brand Name // Merchant // User
        public string? Logo_Url                        { get; set; }
        public string? Logo_Text                       { get; set; }

        //Pass Name // Title on Apple Pass eg. gift card, card holder name 
        public string? Localized_Name                  { get; set; }
        public string? Card_holder_Name                { get; set; }
        public string? Card_Holder_Title               { get; set; }
        //Front Field
        public string? Barcode_Type                    { get; set; }
        public string? Barcode_Format                  { get; set; }
        public string? Code_Type                       { get; set; }

        //Redemption part (Back Fields)
        public string? Recipient_Name                  { get; set; }
        public string? Sender_Name                     { get; set; }
        public string? Issuer                          { get; set; }
        public string? Email                           { get; set; }
        public string? Phone                           { get; set; }
        public string? Address                         { get; set; }

        public string? Webiste                         { get; set; }
        public string? Web_Service_URL                 { get; set; }
        
        public string? Privacy_Policy                  { get; set; }
        public string? Terms_And_Conditions            { get; set; }

        public string? Message                         { get; set; }
        public string? Description                     { get; set; }
    }
}
