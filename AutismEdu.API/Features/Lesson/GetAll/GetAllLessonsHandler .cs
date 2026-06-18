using AutismEdu.API.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutismEdu.API.Features.Lesson.GetAll
{
    public class GetAllLessonsHandler : IRequestHandler<GetAllLessonsQuery, IEnumerable<LessonDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllLessonsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<LessonDto>> Handle(GetAllLessonsQuery request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.GetRepository<Models.Lesson>();

            var query = repo.GetAllAsync(trackChanges: false);

            // ISSUE 1 FIX: Data isolation — specialists only see their own lessons
            if (request.CurrentUserId.HasValue &&
                string.Equals(request.CurrentRole, "specialist", StringComparison.OrdinalIgnoreCase))
            {
                var userId = request.CurrentUserId.Value;
                query = query.Where(x => x.SpecialistId == userId || x.CreatedBy == userId);
            }

            var list = await query.ToListAsync(cancellationToken);

            return list.Select(lesson => new LessonDto
            {
                Id = lesson.Id,
                Title = lesson.Title,
                Type = lesson.Type,
                MediaUrl = lesson.MediaUrl,
                Description = lesson.Description,
                AudioUrl = lesson.AudioUrl,
                SpeechUrl = lesson.SpeechUrl
            });
        }
    }
}
