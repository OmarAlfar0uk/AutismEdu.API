using AutismEdu.API.Features.Performance;
using MediatR;

namespace AutismEdu.API.Shared
{
    public class GetLessonPerformanceQuery : IRequest<PerformanceDto?>
    {
        public Guid ChildId { get; set; }
        public Guid LessonId { get; set; }
    }
}
