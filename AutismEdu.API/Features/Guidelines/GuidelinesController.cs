using AutismEdu.API.Features.Guidelines.Create;
using AutismEdu.API.Features.Guidelines.Delete;
using AutismEdu.API.Features.Guidelines.Download;
using AutismEdu.API.Features.Guidelines.GetAll;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutismEdu.API.Features.Guidelines
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuidelinesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GuidelinesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Guidelines — Public, no auth required
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int limit = 6)
        {
            var result = await _mediator.Send(new GetAllGuidelinesQuery
            {
                Page = page,
                Limit = limit
            });
            return Ok(result);
        }

        // GET: api/Guidelines/{id}/download — Any authenticated user
        [HttpGet("{id:guid}/download")]
        [Authorize]
        public async Task<IActionResult> Download(Guid id)
        {
            var result = await _mediator.Send(new DownloadGuidelineQuery { Id = id });

            if (result == null)
                return NotFound(new { status = "error", code = 404, message = "Guideline not found.", timestamp = DateTime.UtcNow });

            return Ok(result);
        }

        // POST: api/Guidelines — Specialist only
        [HttpPost]
        [Authorize(Roles = "specialist,Admin")]
        public async Task<IActionResult> Create([FromForm] CreateGuidelineCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(201, result);
        }

        // DELETE: api/Guidelines/{id} — Specialist only
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "specialist,Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteGuidelineCommand { Id = id });

            if (!result)
                return NotFound(new { status = "error", code = 404, message = "Guideline not found.", timestamp = DateTime.UtcNow });

            return Ok(new { message = "Guideline deleted successfully" });
        }
    }
}
