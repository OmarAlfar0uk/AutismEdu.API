using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using AutismEdu.API.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutismEdu.API.Features.Performance.GetAll
{
    public class GetChildPerformanceHandler
       : IRequestHandler<GetChildPerformanceQuery, IEnumerable<PerformanceDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetChildPerformanceHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PerformanceDto>> Handle(GetChildPerformanceQuery request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.GetRepository<PerformanceRecord>();

            var list = await repo
                .GetAllAsync(false)
                .Where(p => p.ChildId == request.ChildId)
                .OrderByDescending(p => p.Date)
                .ToListAsync(cancellationToken);

            return list.Select(p => new PerformanceDto
            {
                Id = p.Id,
                ChildId = p.ChildId,
                LessonId = p.LessonId,
                Score = p.Score,
                TimeSpent = p.TimeSpent,
                Date = p.Date
            });
        }
    }
}
