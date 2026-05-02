using AutismEdu.API.Features.Dashboard.Overview;
using AutismEdu.API.Features.Dashboard.Patients;
using AutismEdu.API.Features.Dashboard.Schedule;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutismEdu.API.Features.Dashboard
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "specialist,Admin")]
    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("overview")]
        public async Task<IActionResult> GetOverview()
        {
            var result = await _mediator.Send(new GetOverviewQuery());
            return Ok(result);
        }

        [HttpGet("patients/active")]
        public async Task<IActionResult> GetActivePatients([FromQuery] int limit = 10)
        {
            var result = await _mediator.Send(new GetActivePatientsQuery { Limit = limit });
            return Ok(result);
        }

        [HttpGet("schedule/today")]
        public async Task<IActionResult> GetTodaySchedule()
        {
            var result = await _mediator.Send(new GetTodayScheduleQuery());
            return Ok(result);
        }
    }
}
