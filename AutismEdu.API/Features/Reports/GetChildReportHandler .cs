using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using AutismEdu.API.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;



namespace AutismEdu.API.Features.Reports
{
    public class GetChildReportHandler : IRequestHandler<GetChildReportQuery, ChildReportDto>
    {
        private readonly IUnitOfWork _uow;

        public GetChildReportHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ChildReportDto> Handle(GetChildReportQuery request, CancellationToken cancellationToken)
        {
            var childRepo = _uow.GetRepository<ChildProfile>();
            var perfRepo = _uow.GetRepository<PerformanceRecord>();
            var actRepo = _uow.GetRepository<Models.ChildActivity>();

            // Child info
            var child = await childRepo.GetByIdAsync(request.ChildId);
            if (child == null) return null;

            // Lessons completed
            var lessonsCompleted = await perfRepo.GetAllAsync()
                .Where(x => x.ChildId == request.ChildId)
                .CountAsync(cancellationToken);

            // Activities completed
            var activitiesCompleted = await actRepo.GetAllAsync()
                .Where(x => x.ChildId == request.ChildId)
                .CountAsync(cancellationToken);

            // Average score
            var avgScore = await perfRepo.GetAllAsync()
                .Where(x => x.ChildId == request.ChildId)
                .AverageAsync(x => (double?)x.Score) ?? 0;

            // Total time spent
            var totalTime = await perfRepo.GetAllAsync()
                .Where(x => x.ChildId == request.ChildId)
                .SumAsync(x => x.TimeSpent ?? 0);

            // Last activity date
            var lastActivityDate = await perfRepo.GetAllAsync()
                .Where(x => x.ChildId == request.ChildId)
                .OrderByDescending(x => x.Date)
                .Select(x => x.Date)
                .FirstOrDefaultAsync();

            return new ChildReportDto
            {
                Id = request.ChildId,
                ChildId = request.ChildId,
                PatientId = request.ChildId,
                Name = child.Name,
                LessonsCompleted = lessonsCompleted,
                ActivitiesCompleted = activitiesCompleted,
                AverageScore = avgScore,
                TotalTimeSpent = totalTime,
                LastActivityDate = lastActivityDate
            };
        }
    }
}
