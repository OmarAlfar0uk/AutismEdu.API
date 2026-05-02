using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using MediatR;
using System.Text.Json;

namespace AutismEdu.API.Features.Reports.Create
{
    public class CreateReportHandler : IRequestHandler<CreateReportCommand, ReportDto>
    {
        private readonly IUnitOfWork _uow;

        public CreateReportHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ReportDto> Handle(CreateReportCommand request, CancellationToken cancellationToken)
        {
            var childRepo = _uow.GetRepository<ChildProfile>();
            var child = await childRepo.GetByIdAsync(request.IdStudent);
            if (child == null)
                throw new KeyNotFoundException("Child not found.");

            var report = new Report
            {
                StudentId = request.IdStudent,
                Frequency = request.Frequency,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Skills = JsonSerializer.Serialize(request.Skills.Select(s => s.ToString()).ToList()),
                ClinicalObservations = request.ObservationsClinical,
                Recommendations = request.Recommendations,
                Status = request.Status
            };

            var repo = _uow.GetRepository<Report>();
            await repo.CreateAsync(report);
            await _uow.SaveChangesAsync();

            return new ReportDto
            {
                Id = report.Id,
                StudentId = report.StudentId,
                StudentName = child.Name,
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
