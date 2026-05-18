using AutismEdu.API.Models.Enums;

namespace AutismEdu.API.Features.Patients
{
    public class PatientDto
    {
        public Guid Id { get; set; }
        public Guid ChildId { get; set; }
        public Guid PatientId { get; set; }
        public string Name { get; set; } = default!;
        public int Age { get; set; }
        public string? FocusArea { get; set; }
        public List<string> SkillTags { get; set; } = new();
        public string? Status { get; set; }
        public string? ParentEmail { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
