using AutismEdu.API.Contracts;
using AutismEdu.API.Features.Dashboard.Overview;
using AutismEdu.API.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AutismEdu.API.Features.Dashboard.Schedule
{
    public class GetTodayScheduleHandler : IRequestHandler<GetTodayScheduleQuery, TodayScheduleDto>
    {
        private readonly IUnitOfWork _uow;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetTodayScheduleHandler(IUnitOfWork uow, IHttpContextAccessor httpContextAccessor)
        {
            _uow = uow;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TodayScheduleDto> Handle(GetTodayScheduleQuery request, CancellationToken cancellationToken)
        {
            var userIdStr = _httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var specialistId))
            {
                throw new UnauthorizedAccessException("Specialist ID not found in token.");
            }

            var apptRepo = _uow.GetRepository<Appointment>();
            var today = DateTime.UtcNow.Date;

            var appointments = await apptRepo.GetAllAsync()
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

            return new TodayScheduleDto
            {
                Date = today.ToString("yyyy-MM-dd"),
                Appointments = appointments
            };
        }
    }
}
