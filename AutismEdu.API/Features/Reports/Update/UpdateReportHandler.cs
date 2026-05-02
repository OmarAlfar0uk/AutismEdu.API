using AutismEdu.API.Contracts;
using AutismEdu.API.Exceptions;
using AutismEdu.API.Models;
using AutismEdu.API.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AutismEdu.API.Features.Reports.Update
{
    public class UpdateReportHandler : IRequestHandler<UpdateReportCommand, ReportDto>
    {
        private readonly IUnitOfWork _uow;

        public UpdateReportHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ReportDto> Handle(UpdateReportCommand request, CancellationToken cancellationToken)
        {
            var repo = _uow.GetRepository<Report>();
            var report = await repo.GetByIdAsync(request.Id);

            if (report == null)
                throw new KeyNotFoundException("Report not found.");

            // Business rule: warn if editing a published report
            if (report.Status == ReportStatus.Published)
                throw new ConflictException("This report is already published. Editing a published report may affect shared data. Please confirm before proceeding.");

            // Partial update
            if (request.ObservationsClinical != null)
                report.ClinicalObservations = request.ObservationsClinical;

            if (request.Recommendations != null)
                report.Recommendations = request.Recommendations;

            if (request.Skills != null)
                report.Skills = JsonSerializer.Serialize(request.Skills.Select(s => s.ToString()).ToList());

            if (request.Status.HasValue)
                report.Status = request.Status.Value;

            repo.Update(report);
            await _uow.SaveChangesAsync();

            // Fetch child name
            var childRepo = _uow.GetRepository<ChildProfile>();
            var child = await childRepo.GetByIdAsync(report.StudentId);

            return new ReportDto
            {
                Id = report.Id,
                StudentId = report.StudentId,
                StudentName = child?.Name,
                Frequency = report.Frequency.ToString(),
                StartDate = report.StartDate,
                EndDate = report.EndDate,
                Skills = JsonSerializer.Deserialize<List<string>>(report.Skills) ?? new(),
                ClinicalObservations = report.ClinicalObservations,
                Recommendations = report.Recommendations,
                Status = report.Status.ToString(),
                CreatedAt = report.CreatedAt
            };
        }
    }
}
