using MediatR;

namespace AutismEdu.API.Features.Lesson.Update
{
    public class UpdateLessonCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string Type { get; set; } = default!;
        public string MediaUrl { get; set; } = default!;
        public string? Description { get; set; }
        public string? AudioUrl { get; set; }
    }
}
