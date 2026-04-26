using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using AutismEdu.API.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutismEdu.API.Features.Performance.GetById
{
    public class GetChildPerformanceStatsHandler : IRequestHandler<GetChildPerformanceStatsQuery, PerformanceStatsDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetChildPerformanceStatsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PerformanceStatsDto> Handle(GetChildPerformanceStatsQuery request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.GetRepository<PerformanceRecord>();
            var lessonsRepo = _unitOfWork.GetRepository<AutismEdu.API.Models.Lesson>();


            var data = await repo
                .GetAllAsync(false)
                .Where(p => p.ChildId == request.ChildId)
                .ToListAsync(cancellationToken);

            if (!data.Any())
                return new PerformanceStatsDto();

            var lessonTitles = await lessonsRepo
                .GetAllAsync(false)
                .ToDictionaryAsync(l => l.Id, l => l.Title, cancellationToken);

            return new PerformanceStatsDto
            {
                TotalLessonsCompleted = data.Count,
                AverageScore = data.Average(d => d.Score),
                TotalTimeSpent = data.Sum(d => d.TimeSpent ?? 0),
                BestLesson = lessonTitles[data.OrderByDescending(d => d.Score).First().LessonId],
                WeakestLesson = lessonTitles[data.OrderBy(d => d.Score).First().LessonId]
            };
        }
    }
}
