using MediatR;

namespace AutismEdu.API.Features.Reports.Delete
{
    public class DeleteReportCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
