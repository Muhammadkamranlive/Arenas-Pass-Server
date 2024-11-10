using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class GiftCard : WalletPass
    {
        public string Currency_Code      { get; set; }
        public string Recipient_Name     { get; set; }
        public string Sender_Name        { get; set; }
        public string Message            { get; set; }
        public string Barcode_Type       { get; set; }
        public string Barcode_Format     { get; set; }
        public DateTime? Expiration_Date { get; set; }
        public DateTime? Relevant_Date   { get; set; }
        public decimal? Balance          { get; set; }
    }

}
