using AutismEdu.API.Features.Dashboard.Overview;
using MediatR;

namespace AutismEdu.API.Features.Dashboard.Schedule
{
    public class TodayScheduleDto
    {
        public string Date { get; set; } = default!;
        public List<TodayAppointmentDto> Appointments { get; set; } = new();
    }
}
