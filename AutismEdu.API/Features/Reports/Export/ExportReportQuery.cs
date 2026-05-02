using MediatR;

namespace AutismEdu.API.Features.Reports.Export
{
    public enum ExportFormat
    {
        Pdf,
        Csv
    }

    public class ExportReportQuery : IRequest<ExportReportResult?>
    {
        public Guid Id { get; set; }
        public ExportFormat Format { get; set; }
    }

    public class ExportReportResult
    {
        public byte[] FileContent { get; set; } = default!;
        public string ContentType { get; set; } = default!;
        public string FileName { get; set; } = default!;
    }
}
