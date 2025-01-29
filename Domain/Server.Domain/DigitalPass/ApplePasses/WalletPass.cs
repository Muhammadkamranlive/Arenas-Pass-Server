using System;
using System.Linq;
using System.Text;
using Server.Configurations;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class WalletPass
    {
        public int Id                                  { get; set; }
        public string Type                             { get; set; }
        public string Background_Color                 { get; set; }
        public string Foreground_Color                 { get; set; }
        public string Label_Color                      { get; set; }
        public string Organization_Name                { get; set; }
        public string Serial_Number                    { get; set; }
        public int    TenantId                         { get; set; } = 4;
        public string? Logo_Url                        { get; set; }
        public string? Logo_Text                       { get; set; }
        public string? Localized_Name                  { get; set; }
        public string? Card_holder_Name                { get; set; }
        public string? Card_Holder_Title               { get; set; }
        public string? Code_Type                       { get; set; }
        public DateTime? Expiration_Date               { get; set; }
        public string? Recipient_Name                  { get; set; }
        public string? Sender_Name                     { get; set; }
        public string? Issuer                          { get; set; }
        public string? Email                           { get; set; }
        public string? Phone                           { get; set; }
        public string? Address                         { get; set; }
        public string? Privacy_Policy                  { get; set; }
        public string? Terms_And_Conditions            { get; set; }
        public string? Message                         { get; set; }
        public string? Description                     { get; set; }
        public string  Pass_Status                     { get; set; }= Pass_Redemption_Status_GModel.Template;



    }

}
