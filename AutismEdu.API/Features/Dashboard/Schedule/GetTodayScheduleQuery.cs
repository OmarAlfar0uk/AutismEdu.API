using MediatR;

namespace AutismEdu.API.Features.Dashboard.Schedule
{
    public class GetTodayScheduleQuery : IRequest<TodayScheduleDto>
    {
    }
}
