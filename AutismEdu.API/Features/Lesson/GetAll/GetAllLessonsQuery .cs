using MediatR;

namespace AutismEdu.API.Features.Lesson.GetAll
{
    public class GetAllLessonsQuery : IRequest<IEnumerable<LessonDto>>
    {
        /// <summary>
        /// Current authenticated user ID (from JWT).
        /// </summary>
        public Guid? CurrentUserId { get; set; }

        /// <summary>
        /// Current user role (from JWT). Used for data isolation.
        /// </summary>
        public string? CurrentRole { get; set; }
    }
}
