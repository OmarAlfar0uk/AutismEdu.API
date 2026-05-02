using AutismEdu.API.Models.Enums;

namespace AutismEdu.API.Features.Reports
{
    public class ReportDto
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public string? StudentName { get; set; }
        public string Frequency { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<string> Skills { get; set; } = new();
        public string? ClinicalObservations { get; set; }
        public string? Recommendations { get; set; }
        public string Status { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}
