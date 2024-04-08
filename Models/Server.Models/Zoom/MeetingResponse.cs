using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Models
{
    public class MeetingResponse
    {
        public long id             { get; set; }
        public string password     { get; set; }
        public DateTime start_time { get; set; }
        public int duration        { get; set; }
        public string timezone     { get; set; }
        public DateTime created_at { get; set; }
        public string start_url    { get; set; }
        public string join_url     { get; set; }
    }
}
