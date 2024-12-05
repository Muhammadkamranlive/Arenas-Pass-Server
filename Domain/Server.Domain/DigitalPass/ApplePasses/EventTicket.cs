using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class EventTicket : WalletPass
    {
        public string EventName    { get; set; }
        public string VenueName    { get; set; }
        public string SeatInfo     { get; set; }  
        public string TicketNumber { get; set; }
        public string EntryGate    { get; set; }
        public string Ticket_Type  { get; set; }
    }

}
