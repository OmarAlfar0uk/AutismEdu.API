using MediatR;

namespace AutismEdu.API.Features.Lesson.Add
{
    public class CreateLessonCommand : IRequest<Guid>
    {
        public string Title { get; set; } = default!;
        public string Type { get; set; } = default!;
        public string MediaUrl { get; set; } = default!;
        public string? Description { get; set; }
        public string? AudioUrl { get; set; }
    }
}
