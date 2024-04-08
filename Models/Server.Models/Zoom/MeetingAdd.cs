using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Models
{
    public class MeetingAdd
    {
        public string? meetingBanner { get; set; }
        public string Topic         { get; set; }
        public DateTime Date        { get; set; }
        public string description   { get; set; }
        public string meetingType   { get; set; }
        public string Email         { get; set; }
        public string recieverId    { get; set; }
        public string senderId      { get; set; }
    }
}
