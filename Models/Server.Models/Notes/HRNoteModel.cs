namespace Server.Models
{
    public class HRNoteModel
    {
        public Guid Id              { get; set; }
        public string UserId        { get; set; }
        public string HRId          { get; set; }
        public string Note          { get; set; }
    }
}
