using MediatR;

namespace AutismEdu.API.Features.Lesson.GetById
{
    public class GetLessonByIdQuery : IRequest<LessonDto?>
    {
        public Guid Id { get; set; }
    }
}
