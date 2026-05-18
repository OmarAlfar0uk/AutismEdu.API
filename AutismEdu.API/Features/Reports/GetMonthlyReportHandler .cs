using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using AutismEdu.API.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace AutismEdu.API.Features.Reports
{
    public class GetMonthlyReportHandler
      : IRequestHandler<GetMonthlyReportQuery, ChildReportDto>
    {
        private readonly IUnitOfWork _uow;

        public GetMonthlyReportHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ChildReportDto> Handle(GetMonthlyReportQuery request, CancellationToken cancellationToken)
        {
            var monthAgo = DateTime.UtcNow.AddDays(-30);

            var perfRepo = _uow.GetRepository<PerformanceRecord>();

            var stats = await perfRepo.GetAllAsync()
                .Where(x => x.ChildId == request.ChildId && x.Date >= monthAgo)
                .ToListAsync();

            return new ChildReportDto
            {
                Id = request.ChildId,
                ChildId = request.ChildId,
                PatientId = request.ChildId,
                LessonsCompleted = stats.Count,
                AverageScore = stats.Any() ? stats.Average(x => x.Score) : 0,
                TotalTimeSpent = stats.Sum(x => x.TimeSpent ?? 0),
                LastActivityDate = stats.OrderByDescending(x => x.Date).Select(x => x.Date).FirstOrDefault()
            };
        }
    }

}
