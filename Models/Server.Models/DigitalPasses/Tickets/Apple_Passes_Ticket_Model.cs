using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Models
{
    public class Apple_Passes_Ticket_Model:WalletPassModel
    {
        public string EventName    { get; set; }
        public string VenueName    { get; set; }
        public string SeatInfo     { get; set; }
        public string TicketNumber { get; set; }
        public string EntryGate    { get; set; }
        public string Ticket_Type  { get; set; }
    }

}
