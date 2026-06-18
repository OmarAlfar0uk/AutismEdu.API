using System;

namespace AutismEdu.API.Models
{
    public class ChildGuideline : BaseEntity
    {
        public Guid GuidelineId { get; set; }
        public Guideline Guideline { get; set; } = default!;
        
        public Guid ChildId { get; set; }
        public ChildProfile Child { get; set; } = default!;
        
        public Guid AssignedBy { get; set; }
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    }
}
