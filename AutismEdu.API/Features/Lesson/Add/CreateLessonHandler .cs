using AutismEdu.API.Contracts;
using MediatR;

namespace AutismEdu.API.Features.Lesson.Add
{
    public class CreateLessonHandler : IRequestHandler<CreateLessonCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateLessonHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateLessonCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.GetRepository<Models.Lesson>();

            var lesson = new Models.Lesson
            {
                Title = request.Title,
                Type = request.Type,
                MediaUrl = request.MediaUrl,
                Description = request.Description,
                AudioUrl = request.AudioUrl
            };

            await repo.CreateAsync(lesson);
            await _unitOfWork.SaveChangesAsync();

            return lesson.Id;
        }
    }
}
