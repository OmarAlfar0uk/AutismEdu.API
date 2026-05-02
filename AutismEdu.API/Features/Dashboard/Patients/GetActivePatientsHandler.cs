using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using AutismEdu.API.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutismEdu.API.Features.Dashboard.Patients
{
    public class GetActivePatientsHandler : IRequestHandler<GetActivePatientsQuery, List<ActivePatientDto>>
    {
        private readonly IUnitOfWork _uow;

        public GetActivePatientsHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<List<ActivePatientDto>> Handle(GetActivePatientsQuery request, CancellationToken cancellationToken)
        {
            var childRepo = _uow.GetRepository<ChildProfile>();
            var apptRepo = _uow.GetRepository<Appointment>();

            var patients = await childRepo.GetAllAsync()
                .Where(p => p.Status == PatientStatus.InProgress)
                .Take(request.Limit)
                .ToListAsync(cancellationToken);

            var result = new List<ActivePatientDto>();

            foreach (var p in patients)
            {
                var lastSession = await apptRepo.GetAllAsync()
                    .Where(a => a.ChildId == p.Id && a.Status == AppointmentStatus.Completed)
                    .OrderByDescending(a => a.TimeStart)
                    .FirstOrDefaultAsync(cancellationToken);

                result.Add(new ActivePatientDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Age = p.Age,
                    FocusArea = p.FocusArea,
                    LastSessionDate = lastSession?.TimeStart,
                    ProgressStatus = p.Status?.ToString(),
                    CurrentLevel = "Intermediate" // Mock or derive if LessonLevel exists for this child
                });
            }

            return result;
        }
    }
}
