using AutismEdu.API.Models.Enums;

namespace AutismEdu.API.Features.Lesson.ChildLevel
{
    public class ChildLevelDto
    {
        public Guid LessonId { get; set; }
        public Guid ChildId { get; set; }
        public string Level { get; set; } = default!;
        public int MasteryPercent { get; set; }
        public string? Notes { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
