using AutismEdu.API.Models.Enums;
using MediatR;

namespace AutismEdu.API.Features.Reports.Create
{
    public class CreateReportCommand : IRequest<ReportDto>
    {
        public Guid IdStudent { get; set; }
        public ReportFrequency Frequency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<SkillType> Skills { get; set; } = new();
        public string? ObservationsClinical { get; set; }
        public string? Recommendations { get; set; }
        public ReportStatus Status { get; set; } = ReportStatus.Draft;
    }
}
