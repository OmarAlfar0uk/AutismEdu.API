using AutismEdu.API.Contracts;
using MediatR;

namespace AutismEdu.API.Features.Lesson.GetById
{
    public class GetLessonByIdHandler : IRequestHandler<GetLessonByIdQuery, LessonDto?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetLessonByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<LessonDto?> Handle(GetLessonByIdQuery request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.GetRepository<Models.Lesson>();

            var lesson = await repo.GetByIdAsync(request.Id);
            if (lesson is null)
                return null;

            return new LessonDto
            {
                Id = lesson.Id,
                Title = lesson.Title,
                Type = lesson.Type,
                MediaUrl = lesson.MediaUrl,
                Description = lesson.Description,
                AudioUrl = lesson.AudioUrl,
                SpeechUrl = lesson.SpeechUrl
            };
        }
    }

}
