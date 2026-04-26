using AutismEdu.API.Features.Reports;
using MediatR;

namespace AutismEdu.API.Shared
{
    public record GetWeeklyReportQuery(Guid ChildId) : IRequest<ChildReportDto>;
}
