using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using CsvHelper;
using MediatR;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;
using System.Text.Json;

namespace AutismEdu.API.Features.Reports.Export
{
    public class ExportReportHandler : IRequestHandler<ExportReportQuery, ExportReportResult?>
    {
        private readonly IUnitOfWork _uow;

        public ExportReportHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ExportReportResult?> Handle(ExportReportQuery request, CancellationToken cancellationToken)
        {
            var repo = _uow.GetRepository<Report>();
            var report = await repo.GetByIdAsync(request.Id);

            if (report == null)
                return null;

            var childRepo = _uow.GetRepository<ChildProfile>();
            var child = await childRepo.GetByIdAsync(report.StudentId);

            var skills = JsonSerializer.Deserialize<List<string>>(report.Skills) ?? new();

            return request.Format switch
            {
                ExportFormat.Pdf => GeneratePdf(report, child, skills),
                ExportFormat.Csv => GenerateCsv(report, child, skills),
                _ => null
            };
        }

        private ExportReportResult GeneratePdf(Report report, ChildProfile? child, List<string> skills)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header().Column(col =>
                    {
                        col.Item().Text("AutismEdu — Student Report")
                            .FontSize(20).Bold().FontColor(Colors.Blue.Darken2);
                        col.Item().PaddingTop(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                    });

                    page.Content().PaddingVertical(15).Column(col =>
                    {
                        col.Spacing(8);

                        col.Item().Text($"Student: {child?.Name ?? "N/A"}").Bold();
                        col.Item().Text($"Report Period: {report.StartDate:yyyy-MM-dd} to {report.EndDate:yyyy-MM-dd}");
                        col.Item().Text($"Frequency: {report.Frequency}");
                        col.Item().Text($"Status: {report.Status}");

                        col.Item().PaddingTop(10).Text("Skills Assessed:").Bold();
                        foreach (var skill in skills)
                        {
                            col.Item().PaddingLeft(15).Text($"• {skill}");
                        }

                        if (!string.IsNullOrEmpty(report.ClinicalObservations))
                        {
                            col.Item().PaddingTop(10).Text("Clinical Observations:").Bold();
                            col.Item().PaddingLeft(15).Text(report.ClinicalObservations);
                        }

                        if (!string.IsNullOrEmpty(report.Recommendations))
                        {
                            col.Item().PaddingTop(10).Text("Recommendations:").Bold();
                            col.Item().PaddingLeft(15).Text(report.Recommendations);
                        }

                        col.Item().PaddingTop(15).Text($"Generated: {DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC")
                            .FontSize(9).FontColor(Colors.Grey.Medium);
                    });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Page ");
                        text.CurrentPageNumber();
                        text.Span(" of ");
                        text.TotalPages();
                    });
                });
            });

            using var stream = new MemoryStream();
            document.GeneratePdf(stream);

            return new ExportReportResult
            {
                FileContent = stream.ToArray(),
                ContentType = "application/pdf",
                FileName = $"Report_{report.Id}.pdf"
            };
        }

        private ExportReportResult GenerateCsv(Report report, ChildProfile? child, List<string> skills)
        {
            using var stream = new MemoryStream();
            using var writer = new StreamWriter(stream);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            // Write header
            csv.WriteField("ReportId");
            csv.WriteField("StudentName");
            csv.WriteField("Frequency");
            csv.WriteField("StartDate");
            csv.WriteField("EndDate");
            csv.WriteField("Skills");
            csv.WriteField("ClinicalObservations");
            csv.WriteField("Recommendations");
            csv.WriteField("Status");
            csv.WriteField("CreatedAt");
            csv.NextRecord();

            // Write data row
            csv.WriteField(report.Id);
            csv.WriteField(child?.Name ?? "N/A");
            csv.WriteField(report.Frequency.ToString());
            csv.WriteField(report.StartDate.ToString("yyyy-MM-dd"));
            csv.WriteField(report.EndDate.ToString("yyyy-MM-dd"));
            csv.WriteField(string.Join("; ", skills));
            csv.WriteField(report.ClinicalObservations ?? "");
            csv.WriteField(report.Recommendations ?? "");
            csv.WriteField(report.Status.ToString());
            csv.WriteField(report.CreatedAt.ToString("yyyy-MM-dd HH:mm"));
            csv.NextRecord();

            writer.Flush();

            return new ExportReportResult
            {
                FileContent = stream.ToArray(),
                ContentType = "text/csv",
                FileName = $"Report_{report.Id}.csv"
            };
        }
    }
}
