using MediatR;

namespace AutismEdu.API.Features.Performance.Add
{
    public class AddPerformanceCommand : IRequest<Guid>
    {
        public Guid ChildId { get; set; }
        public Guid LessonId { get; set; }

        public int Score { get; set; }
        public int Attempts { get; set; }
        public int? TimeSpent { get; set; }
    }
}
