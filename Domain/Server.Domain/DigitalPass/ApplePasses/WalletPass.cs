using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class WalletPass
    {
        public int Id                                  { get; set; }
        public string Type                             { get; set; }  
        public string? Logo                            { get; set; }
        public string Background_Color                 { get; set; }
        public string Foreground_Color                 { get; set; }
        public string Label_Color                      { get; set; }
        public string Organization_Name                { get; set; }
        public string Serial_Number                    { get; set; }  
        public string? Localized_Name                  { get; set; }
        public string? Terms_And_Conditions            { get; set; }
        public string? Description                     { get; set; }  
        public string? Web_Service_URL                 { get; set; }
        public string? Authentication_Token            { get; set; }
        public byte[] Apple_Pass                       { get; set; }
        public int    TenantId                         { get; set; }=4;

    }

}
