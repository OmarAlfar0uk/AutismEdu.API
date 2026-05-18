using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using AutismEdu.API.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace AutismEdu.API.Features.Reports
{
    public class GetWeeklyReportHandler : IRequestHandler<GetWeeklyReportQuery, ChildReportDto>
    {
        private readonly IUnitOfWork _uow;

        public GetWeeklyReportHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ChildReportDto> Handle(GetWeeklyReportQuery request, CancellationToken cancellationToken)
        {
            var oneWeekAgo = DateTime.UtcNow.AddDays(-7);

            var perfRepo = _uow.GetRepository<PerformanceRecord>();

            var weeklyStats = await perfRepo.GetAllAsync()
                .Where(x => x.ChildId == request.ChildId && x.Date >= oneWeekAgo)
                .ToListAsync();

            return new ChildReportDto
            {
                Id = request.ChildId,
                ChildId = request.ChildId,
                PatientId = request.ChildId,
                LessonsCompleted = weeklyStats.Count,
                AverageScore = weeklyStats.Any() ? weeklyStats.Average(x => x.Score) : 0,
                TotalTimeSpent = weeklyStats.Sum(x => x.TimeSpent ?? 0),
                LastActivityDate = weeklyStats.OrderByDescending(x => x.Date).Select(x => x.Date).FirstOrDefault()
            };
        }
    }
}
