namespace Server.Domain
{
    public class Education
    {
        public Guid Id                  { get; set; }
        public string userId            { get; set; }
        public string SchoolName        { get; set; }
        public string Degree            { get; set; }
        public string FieldOfStudy      { get; set; }
        public int YearOfCompletion     { get; set; }
        public double GPA               { get; set; }
        public string? Interests        { get; set; }
        public string? Notes            { get; set; }
        public int TenantId             { get; set; } = 1;
    }
}
