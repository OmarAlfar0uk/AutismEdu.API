using AutismEdu.API.Models.Enums;

namespace AutismEdu.API.Models
{
    public class Report : BaseEntity
    {
        public Guid StudentId { get; set; }
        public ChildProfile Student { get; set; } = default!;

        public ReportFrequency Frequency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        /// <summary>
        /// JSON-serialized array of SkillType enums
        /// </summary>
        public string Skills { get; set; } = "[]";

        public string? ClinicalObservations { get; set; }
        public string? Recommendations { get; set; }

        public ReportStatus Status { get; set; } = ReportStatus.Draft;
    }
}
