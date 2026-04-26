using MediatR;

namespace AutismEdu.API.Features.Lesson.GenerateLesso
{
    public class GenerateLessonSpeechCommand : IRequest<string>
    {
        public Guid LessonId { get; set; }
    }
}
