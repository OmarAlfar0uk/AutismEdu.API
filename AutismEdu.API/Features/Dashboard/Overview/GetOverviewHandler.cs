using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using AutismEdu.API.Models.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AutismEdu.API.Features.Dashboard.Overview
{
    public class GetOverviewHandler : IRequestHandler<GetOverviewQuery, OverviewDto>
    {
        private readonly IUnitOfWork _uow;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetOverviewHandler(IUnitOfWork uow, IHttpContextAccessor httpContextAccessor)
        {
            _uow = uow;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<OverviewDto> Handle(GetOverviewQuery request, CancellationToken cancellationToken)
        {
            var userIdStr = _httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var specialistId))
            {
                throw new UnauthorizedAccessException("Specialist ID not found in token.");
            }

            var childRepo = _uow.GetRepository<ChildProfile>();
            var apptRepo = _uow.GetRepository<Appointment>();

            var today = DateTime.UtcNow.Date;

            var activePatients = await childRepo.GetAllAsync()
                .Where(p => p.Status == PatientStatus.InProgress)
                .CountAsync(cancellationToken);

            var reviewsPending = await childRepo.GetAllAsync()
                .Where(p => p.Status == PatientStatus.NeedsReview)
                .CountAsync(cancellationToken);

            var sessionsTodayCount = await apptRepo.GetAllAsync()
                .Where(a => a.SpecialistId == specialistId && a.TimeStart.Date == today)
                .CountAsync(cancellationToken);

            var todaysAppointments = await apptRepo.GetAllAsync()
                .Where(a => a.SpecialistId == specialistId && a.TimeStart.Date == today)
                .OrderBy(a => a.TimeStart)
                .Select(a => new TodayAppointmentDto
                {
                    TimeStart = a.TimeStart.ToString("HH:mm"),
                    TimeEnd = a.TimeEnd.ToString("HH:mm"),
                    PatientName = a.Child.Name,
                    FocusArea = a.FocusArea,
                    Status = a.Status.ToString()
                })
                .ToListAsync(cancellationToken);

            return new OverviewDto
            {
                ActivePatients = activePatients,
                SessionsToday = sessionsTodayCount,
                ReviewsPending = reviewsPending,
                WeeklyImpactPercent = 12.5f, // Mock value as per spec format or calculate if logic exists
                TodaysSchedule = todaysAppointments
            };
        }
    }
}
