using AutismEdu.API.Contracts;
using MediatR;

namespace AutismEdu.API.Features.Lesson.Delete
{
    public class DeleteLessonHandler : IRequestHandler<DeleteLessonCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteLessonHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteLessonCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.GetRepository<Models.Lesson>();

            var lesson = await repo.GetByIdAsync(request.Id);
            if (lesson is null)
                throw new Exception("Lesson not found");

            repo.Delete(lesson);
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
