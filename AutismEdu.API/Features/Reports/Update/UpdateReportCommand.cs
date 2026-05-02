using AutismEdu.API.Models.Enums;
using MediatR;

namespace AutismEdu.API.Features.Reports.Update
{
    public class UpdateReportCommand : IRequest<ReportDto>
    {
        public Guid Id { get; set; }
        public string? ObservationsClinical { get; set; }
        public string? Recommendations { get; set; }
        public List<SkillType>? Skills { get; set; }
        public ReportStatus? Status { get; set; }
    }
}
