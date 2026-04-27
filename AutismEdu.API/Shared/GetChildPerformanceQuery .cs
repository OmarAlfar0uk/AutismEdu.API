using AutismEdu.API.Features.Performance;
using MediatR;

namespace AutismEdu.API.Shared
{
    public class GetChildPerformanceQuery : IRequest<IEnumerable<PerformanceDto>>
    {
        public Guid ChildId { get; set; }
    }
}
