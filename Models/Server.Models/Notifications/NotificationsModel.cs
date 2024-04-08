namespace Server.Models
{
    public class NotificationsModel
    {
         public Guid     Id           { get; set; }
        public string   Message      { get; set; }
        public DateTime Timestamp    { get; set; }
        public bool IsRead           { get; set; }
        public string UserId         { get; set; } 
        public string WorkflowStep   { get; set; }
    }
}
