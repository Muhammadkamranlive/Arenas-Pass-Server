using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Models
{
    public class Boarding_Pass_Model:WalletPassModel
    {
        // Core flight details
        public string FlightNumber { get; set; }
        public string DepartureAirportCode { get; set; } // IATA code (e.g., JFK)
        public string DepartureAirportName { get; set; } // Full name of departure airport
        public string ArrivalAirportCode { get; set; } // IATA code (e.g., LAX)
        public string ArrivalAirportName { get; set; } // Full name of arrival airport

        // Timing information
        public DateTime DepartureTime { get; set; } // Scheduled departure time
        public DateTime BoardingTime { get; set; } // Boarding start time

        // Passenger-specific details
        public string GateNumber { get; set; } // Gate number
        public string SeatNumber { get; set; } // Assigned seat number
        public string FrequentFlyerNumber { get; set; } // Loyalty program number

        // Additional fields
        public string Terminal { get; set; } // Terminal number
        public string ClassOfService { get; set; } // e.g., Economy, Business, First Class
        public string BaggageClaimInfo { get; set; } // Baggage claim details
        public string AirlineCode { get; set; } // Airline IATA code (e.g., AA for American Airlines)
        public string AirlineName { get; set; } // Full airline name
        public string PassengerName { get; set; } // Passenger's full name
    }
}
