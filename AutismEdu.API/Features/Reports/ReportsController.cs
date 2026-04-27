using AutismEdu.API.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutismEdu.API.Features.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("child/{childId:guid}")]
        public async Task<IActionResult> GetChildReport(Guid childId)
        {
            var report = await _mediator.Send(new GetChildReportQuery(childId));
            return Ok(report);
        }

        [HttpGet("child/{childId:guid}/weekly")]
        public async Task<IActionResult> GetWeekly(Guid childId)
        {
            var report = await _mediator.Send(new GetWeeklyReportQuery(childId));
            return Ok(report);
        }

        [HttpGet("child/{childId:guid}/monthly")]
        public async Task<IActionResult> GetMonthly(Guid childId)
        {
            var report = await _mediator.Send(new GetMonthlyReportQuery(childId));
            return Ok(report);
        }
    }
}
