namespace AutismEdu.API.Features.Dashboard.Patients
{
    public class ActivePatientDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public int Age { get; set; }
        public string? FocusArea { get; set; }
        public DateTime? LastSessionDate { get; set; }
        public string? ProgressStatus { get; set; }
        public string? CurrentLevel { get; set; }
    }
}
