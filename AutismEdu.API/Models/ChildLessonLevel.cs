using AutismEdu.API.Models.Enums;

namespace AutismEdu.API.Models
{
    public class ChildLessonLevel : BaseEntity
    {
        public Guid ChildId { get; set; }
        public ChildProfile Child { get; set; } = default!;

        public Guid LessonId { get; set; }
        public Lesson Lesson { get; set; } = default!;

        public LessonLevel Level { get; set; }
        public int MasteryPercent { get; set; }
        public string? Notes { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}
