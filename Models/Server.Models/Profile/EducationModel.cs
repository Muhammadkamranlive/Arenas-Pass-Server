
namespace Server.Models
{
    public class EducationModel
    {
        public Guid Id                { get; set; }
        public string userId          { get; set; }
        public string SchoolName      { get; set; }
        public string Degree          { get; set; }
        public string FieldOfStudy    { get; set; }
        public int YearOfCompletion   { get; set; }
        public double GPA             { get; set; }
        public string? Interests       { get; set; }
        public string? Notes           { get; set; }
    }
}
