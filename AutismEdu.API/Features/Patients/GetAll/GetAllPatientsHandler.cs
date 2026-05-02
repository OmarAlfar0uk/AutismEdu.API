using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using AutismEdu.API.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AutismEdu.API.Features.Patients.GetAll
{
    public class GetAllPatientsHandler : IRequestHandler<GetAllPatientsQuery, GetAllPatientsResponse>
    {
        private readonly IUnitOfWork _uow;

        public GetAllPatientsHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<GetAllPatientsResponse> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
        {
            var repo = _uow.GetRepository<ChildProfile>();
            var query = repo.GetAllAsync();

            // Filters
            if (!string.IsNullOrEmpty(request.Status) && request.Status.ToLower() != "all")
            {
                if (Enum.TryParse<PatientStatus>(request.Status, true, out var statusEnum))
                {
                    query = query.Where(p => p.Status == statusEnum);
                }
            }

            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(p => p.Name.Contains(request.Search));
            }

            if (!string.IsNullOrEmpty(request.AgeRange))
            {
                var parts = request.AgeRange.Split('-');
                if (parts.Length == 2 && int.TryParse(parts[0], out int minAge) && int.TryParse(parts[1], out int maxAge))
                {
                    query = query.Where(p => p.Age >= minAge && p.Age <= maxAge);
                }
            }

            var total = await query.CountAsync(cancellationToken);

            // Dashboard aggregates (from query or full set depending on requirements, here using full set for simplicity)
            var allPatients = await repo.GetAllAsync().ToListAsync(cancellationToken);
            var activeCount = allPatients.Count(p => p.Status == PatientStatus.InProgress);
            var reviewsPending = allPatients.Count(p => p.Status == PatientStatus.NeedsReview);
            
            // Sessions today from Appointments
            var apptRepo = _uow.GetRepository<Appointment>();
            var today = DateTime.UtcNow.Date;
            var sessionsToday = await apptRepo.GetAllAsync()
                .Where(a => a.TimeStart.Date == today && a.Status == AppointmentStatus.Scheduled)
                .CountAsync(cancellationToken);

            var patients = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((request.Page - 1) * request.Limit)
                .Take(request.Limit)
                .ToListAsync(cancellationToken);

            var patientDtos = patients.Select(p => new PatientDto
            {
                Id = p.Id,
                Name = p.Name,
                Age = p.Age,
                FocusArea = p.FocusArea,
                SkillTags = string.IsNullOrEmpty(p.SkillTags) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(p.SkillTags) ?? new List<string>(),
                Status = p.Status?.ToString(),
                ParentEmail = p.ParentEmail,
                CreatedAt = p.CreatedAt
            }).ToList();

            return new GetAllPatientsResponse
            {
                Patients = patientDtos,
                Total = total,
                ActiveCount = activeCount,
                ReviewsPending = reviewsPending,
                SessionsToday = sessionsToday,
                Page = request.Page,
                TotalPages = (int)Math.Ceiling(total / (double)request.Limit)
            };
        }
    }
}
