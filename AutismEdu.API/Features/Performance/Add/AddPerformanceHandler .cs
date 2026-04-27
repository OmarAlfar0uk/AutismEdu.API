using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using MediatR;

namespace AutismEdu.API.Features.Performance.Add
{

    public class AddPerformanceHandler : IRequestHandler<AddPerformanceCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddPerformanceHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(AddPerformanceCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.GetRepository<PerformanceRecord>();

            var record = new PerformanceRecord
            {
                ChildId = request.ChildId,
                LessonId = request.LessonId,
                Score = request.Score,
                Attempts = request.Attempts,
                TimeSpent = request.TimeSpent
            };

            await repo.CreateAsync(record);
            await _unitOfWork.SaveChangesAsync();

            return record.Id;
        }
    }
}