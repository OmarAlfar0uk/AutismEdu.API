using AutismEdu.API.Contracts;
using MediatR;

namespace AutismEdu.API.Features.Lesson.Update
{
    public class UpdateLessonHandler : IRequestHandler<UpdateLessonCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateLessonHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateLessonCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.GetRepository<Models.Lesson>();

            var lesson = await repo.GetByIdAsync(request.Id);
            if (lesson is null)
                throw new Exception("Lesson not found");

            lesson.Title = request.Title;
            lesson.Type = request.Type;
            lesson.MediaUrl = request.MediaUrl;
            lesson.Description = request.Description;
            lesson.AudioUrl = request.AudioUrl;

            repo.Update(lesson);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
