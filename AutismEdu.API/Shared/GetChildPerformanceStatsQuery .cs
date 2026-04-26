using AutismEdu.API.Features.Performance;
using MediatR;

namespace AutismEdu.API.Shared
{
    public class GetChildPerformanceStatsQuery : IRequest<PerformanceStatsDto>
    {
        public Guid ChildId { get; set; }
    }
}
