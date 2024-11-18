using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class Ticket : WalletPass
    {
        public string Event_Name         { get; set; }
        public DateTime Event_Date       { get; set; }
        public string Venue              { get; set; }
        public string Seat_Number        { get; set; }
        public string Gate               { get; set; }
        public DateTime? Expiration_Date { get; set; }
        public string Barcode_Type       { get; set; }
        public string Barcode_Format     { get; set; }
    }

}
