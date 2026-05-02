using AutismEdu.API.Models.Enums;
using MediatR;

namespace AutismEdu.API.Features.Lesson.ChildLevel
{
    public class UpsertChildLevelCommand : IRequest<ChildLevelDto>
    {
        public Guid LessonId { get; set; }
        public Guid IdChild { get; set; }
        public LessonLevel Level { get; set; }
        public int PercentMastery { get; set; }
        public string? Notes { get; set; }
    }
}
