using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class BoardingPass : WalletPass
    {
        public string FlightNumber         { get; set; }
        public string DepartureAirportCode { get; set; }
        public string ArrivalAirportCode   { get; set; }
        public DateTime DepartureTime      { get; set; }
        public DateTime BoardingTime       { get; set; }
        public string GateNumber           { get; set; }
        public string SeatNumber           { get; set; }
        public string FrequentFlyerNumber  { get; set; }
    }

}
