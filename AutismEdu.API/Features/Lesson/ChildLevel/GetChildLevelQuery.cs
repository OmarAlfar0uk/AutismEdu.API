using MediatR;

namespace AutismEdu.API.Features.Lesson.ChildLevel
{
    public class GetChildLevelQuery : IRequest<ChildLevelDto?>
    {
        public Guid LessonId { get; set; }
        public Guid? ChildId { get; set; }
    }
}
