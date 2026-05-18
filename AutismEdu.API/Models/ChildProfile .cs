using AutismEdu.API.Models.Enums;

namespace AutismEdu.API.Models
{
    public class ChildProfile : BaseEntity
    {
        public string Name { get; set; } = default!;
        public int Age { get; set; }
        public string? Notes { get; set; }
        public Guid? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        // Patient-specific fields
        public string? FocusArea { get; set; }
        public string? SkillTags { get; set; } // JSON array of strings
        public PatientStatus? Status { get; set; }
        public string? ParentEmail { get; set; }

        public ICollection<PerformanceRecord>? PerformanceRecords { get; set; }
        public ICollection<ChatBotLog>? ChatBotLogs { get; set; }
        public ICollection<ChildActivity>? ChildActivities { get; set; }
        public ICollection<ChildCard>? ChildCards { get; set; }
    }
}
