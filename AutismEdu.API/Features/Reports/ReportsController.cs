using AutismEdu.API.Features.Reports.Create;
using AutismEdu.API.Features.Reports.Delete;
using AutismEdu.API.Features.Reports.Export;
using AutismEdu.API.Features.Reports.Update;
using AutismEdu.API.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        // GET: api/Reports/child/{childId} — existing endpoint
        [HttpGet("child/{childId:guid}")]
        public async Task<IActionResult> GetChildReport(Guid childId)
        {
            var report = await _mediator.Send(new GetChildReportQuery(childId));
            return Ok(report);
        }

        // GET: api/Reports/child/{childId}/weekly — existing endpoint
        [HttpGet("child/{childId:guid}/weekly")]
        public async Task<IActionResult> GetWeekly(Guid childId)
        {
            var report = await _mediator.Send(new GetWeeklyReportQuery(childId));
            return Ok(report);
        }

        // GET: api/Reports/child/{childId}/monthly — existing endpoint
        [HttpGet("child/{childId:guid}/monthly")]
        public async Task<IActionResult> GetMonthly(Guid childId)
        {
            var report = await _mediator.Send(new GetMonthlyReportQuery(childId));
            return Ok(report);
        }

        // POST: api/Reports — Specialist only
        [HttpPost]
        [Authorize(Roles = "specialist,Admin")]
        public async Task<IActionResult> Create([FromBody] CreateReportCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(201, result);
        }

        // PUT: api/Reports/{id} — Specialist only
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "specialist,Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateReportCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // DELETE: api/Reports/{id} — Specialist only
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "specialist,Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteReportCommand { Id = id });

            if (!result)
                return NotFound(new { status = "error", code = 404, message = "Report not found.", timestamp = DateTime.UtcNow });

            return Ok(new { message = "Report deleted" });
        }

        // GET: api/Reports/{id}/export?format=pdf|csv — Specialist or Parent
        [HttpGet("{id:guid}/export")]
        [Authorize(Roles = "specialist,parent")]
        public async Task<IActionResult> Export(Guid id, [FromQuery] ExportFormat format)
        {
            var result = await _mediator.Send(new ExportReportQuery { Id = id, Format = format });

            if (result == null)
                return NotFound(new { status = "error", code = 404, message = "Report not found.", timestamp = DateTime.UtcNow });

            return File(result.FileContent, result.ContentType, result.FileName);
        }
    }
}
