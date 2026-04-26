using MediatR;

namespace AutismEdu.API.Features.Lesson.Delete
{
    public class DeleteLessonCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
