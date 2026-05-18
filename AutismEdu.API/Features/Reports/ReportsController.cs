using AutismEdu.API.Features.Auth.Authorization;
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
        private readonly IChildAuthorizationService _childAuthorizationService;

        public ReportsController(IMediator mediator, IChildAuthorizationService childAuthorizationService)
        {
            _mediator = mediator;
            _childAuthorizationService = childAuthorizationService;
        }

        /// <summary>
        /// Gets a child report. Specialists may access any child; parents may access only children owned by their JWT user id and otherwise receive 404.
        /// </summary>
        [HttpGet("child/{childId:guid}")]
        [Authorize(Roles = "specialist,parent")]
        public async Task<IActionResult> GetChildReport(Guid childId)
        {
            if (!_childAuthorizationService.IsAuthorizedForChild(childId, User))
                return NotFound(Error(404, "Child not found or not accessible"));

            var report = await _mediator.Send(new GetChildReportQuery(childId));
            return Ok(report);
        }

        /// <summary>
        /// Gets a weekly child report. Specialists may access any child; parents may access only children owned by their JWT user id and otherwise receive 404.
        /// </summary>
        [HttpGet("child/{childId:guid}/weekly")]
        [Authorize(Roles = "specialist,parent")]
        public async Task<IActionResult> GetWeekly(Guid childId)
        {
            if (!_childAuthorizationService.IsAuthorizedForChild(childId, User))
                return NotFound(Error(404, "Child not found or not accessible"));

            var report = await _mediator.Send(new GetWeeklyReportQuery(childId));
            return Ok(report);
        }

        /// <summary>
        /// Gets a monthly child report. Specialists may access any child; parents may access only children owned by their JWT user id and otherwise receive 404.
        /// </summary>
        [HttpGet("child/{childId:guid}/monthly")]
        [Authorize(Roles = "specialist,parent")]
        public async Task<IActionResult> GetMonthly(Guid childId)
        {
            if (!_childAuthorizationService.IsAuthorizedForChild(childId, User))
                return NotFound(Error(404, "Child not found or not accessible"));

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
                return NotFound(Error(404, "Report not found."));

            return Ok(new { message = "Report deleted" });
        }

        // GET: api/Reports/{id}/export?format=pdf|csv — Specialist or Parent
        [HttpGet("{id:guid}/export")]
        [Authorize(Roles = "specialist,parent")]
        public async Task<IActionResult> Export(Guid id, [FromQuery] ExportFormat format)
        {
            var result = await _mediator.Send(new ExportReportQuery { Id = id, Format = format });

            if (result == null)
                return NotFound(Error(404, "Report not found."));

            return File(result.FileContent, result.ContentType, result.FileName);
        }

        private static object Error(int code, string message) => new
        {
            status = "error",
            code,
            message,
            timestamp = DateTime.UtcNow.ToString("o")
        };
    }
}
