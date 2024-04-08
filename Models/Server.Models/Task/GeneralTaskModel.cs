namespace Server.Models
{
    public class GeneralTaskModel
    {

        public Guid     Id                     { get; set; }
        public string   Title                  { get; set; }
        public string   Description            { get; set; }
        public DateTime StartDate              { get; set; }
        public DateTime DueDate                { get; set; }
        public string   Type                   { get; set; }
        public string   UserId                 { get; set; }
        public string   Progress               { get; set; }

    }
}
