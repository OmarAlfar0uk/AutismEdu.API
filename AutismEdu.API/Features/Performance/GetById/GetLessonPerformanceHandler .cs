using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using AutismEdu.API.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutismEdu.API.Features.Performance.GetById
{
    public class GetLessonPerformanceHandler : IRequestHandler<GetLessonPerformanceQuery, PerformanceDto?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetLessonPerformanceHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PerformanceDto?> Handle(GetLessonPerformanceQuery request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.GetRepository<PerformanceRecord>();

            var result = await repo
                .GetAllAsync(false)
                .FirstOrDefaultAsync(p => p.ChildId == request.ChildId && p.LessonId == request.LessonId, cancellationToken);

            if (result is null)
                return null;

            return new PerformanceDto
            {
                Id = result.Id,
                ChildId = result.ChildId,
                LessonId = result.LessonId,
                Score = result.Score,
                TimeSpent = result.TimeSpent,
                Date = result.Date
            };
        }
    }
}

