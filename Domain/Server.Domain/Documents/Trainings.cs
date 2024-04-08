namespace Server.Domain
{
    public class Trainings
    {
        public Guid Id { get; set; }

        public string userId { get; set; }
        public string Name { get; set; }
        public string TrainingType { get; set; }

        public string Hours { get; set; }

        public string Credits { get; set; }

        public DateTime Date { get; set; }
        public DateTime? endDate { get; set; }
        public string? School { get; set; }
        public string Notes { get; set; }

        public int TenantId { get; set; } = 1;
    }
}
