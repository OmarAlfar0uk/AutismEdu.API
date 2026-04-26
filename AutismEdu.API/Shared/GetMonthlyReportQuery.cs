using AutismEdu.API.Features.Reports;
using MediatR;

namespace AutismEdu.API.Shared
{
    public record GetMonthlyReportQuery(Guid ChildId) : IRequest<ChildReportDto>;

}
