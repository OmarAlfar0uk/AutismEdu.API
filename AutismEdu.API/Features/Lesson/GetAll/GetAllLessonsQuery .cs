using MediatR;

namespace AutismEdu.API.Features.Lesson.GetAll
{
    public class GetAllLessonsQuery : IRequest<IEnumerable<LessonDto>>
    {
    }
}
