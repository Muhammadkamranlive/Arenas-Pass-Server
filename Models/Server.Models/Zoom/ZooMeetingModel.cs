namespace Server.Models
{
    public class ZooMeetingModel
    {
        public Guid Id                    { get; set; }= new Guid();
        public string MeetingNumber       { get; set; }
        public string password            { get; set; }
        public DateTime start_time        { get; set; }
        public int duration               { get; set; }
        public string timezone            { get; set; }
        public DateTime created_at        { get; set; }
        public string start_url           { get; set; }
        public string join_url            { get; set; }
        public string     Topic           { get; set; }
        public string     email           { get; set; }
        public string recieverId          { get; set; }
        public string senderId            { get; set; }
        public DateTime   Date            { get; set; }
        public string     description     { get; set; }
        public string     meetingstatus   { get; set; }
        public string     meetingBanner   { get; set; }
    }
}
