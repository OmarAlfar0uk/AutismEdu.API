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

            // ISSUE 1 FIX: Data isolation — specialists only see patients they created
            if (request.IsSpecialist && request.SpecialistUserId.HasValue)
            {
                var specialistId = request.SpecialistUserId.Value;
                query = query.Where(p => p.UserId == specialistId || p.CreatedBy == specialistId);
            }

            if (request.ParentUserId.HasValue || !string.IsNullOrWhiteSpace(request.ParentEmail))
            {
                var parentUserId = request.ParentUserId;
                var parentEmail = request.ParentEmail;

                query = query.Where(p =>
                    (parentUserId.HasValue && p.UserId == parentUserId.Value) ||
                    (!string.IsNullOrWhiteSpace(parentEmail) && p.ParentEmail == parentEmail));
            }

            // Filters
            if (!string.IsNullOrEmpty(request.Status) && request.Status.ToLower() != "all")
            {
                if (Enum.TryParse<PatientStatus>(request.Status, true, out var statusEnum))
                {
                    query = query.Where(p => p.Status == statusEnum);
                }
            }

            if (!string.IsNullOrEmpty(request.Search) && request.IsSpecialist)
            {
                query = query.Where(p => p.ParentEmail != null && p.ParentEmail.Contains(request.Search));
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
            var allPatientsQuery = repo.GetAllAsync();

            // ISSUE 1 FIX: Also apply specialist isolation to dashboard aggregates
            if (request.IsSpecialist && request.SpecialistUserId.HasValue)
            {
                var specialistId = request.SpecialistUserId.Value;
                allPatientsQuery = allPatientsQuery.Where(p => p.UserId == specialistId || p.CreatedBy == specialistId);
            }

            if (request.ParentUserId.HasValue || !string.IsNullOrWhiteSpace(request.ParentEmail))
            {
                var parentUserId = request.ParentUserId;
                var parentEmail = request.ParentEmail;

                allPatientsQuery = allPatientsQuery.Where(p =>
                    (parentUserId.HasValue && p.UserId == parentUserId.Value) ||
                    (!string.IsNullOrWhiteSpace(parentEmail) && p.ParentEmail == parentEmail));
            }

            var allPatients = await allPatientsQuery.ToListAsync(cancellationToken);
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
                ChildId = p.Id,
                PatientId = p.Id,
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
