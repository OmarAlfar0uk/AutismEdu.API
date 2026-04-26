using AutismEdu.API.Features.Reports;
using MediatR;

namespace AutismEdu.API.Shared
{
    public record GetChildReportQuery(Guid ChildId) : IRequest<ChildReportDto>;
}
