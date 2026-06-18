using AutismEdu.API.Features.Guidelines.Assign;
using AutismEdu.API.Features.Guidelines.Create;
using AutismEdu.API.Features.Guidelines.Delete;
using AutismEdu.API.Features.Guidelines.Download;
using AutismEdu.API.Features.Guidelines.GetAll;
using AutismEdu.API.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AutismEdu.API.Features.Guidelines
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuidelinesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly Features.Auth.Authorization.IAuthorizationHelper _authHelper;

        public GuidelinesController(IMediator mediator, Features.Auth.Authorization.IAuthorizationHelper authHelper)
        {
            _mediator = mediator;
            _authHelper = authHelper;
        }

        // GET: api/Guidelines — optional authentication with data isolation
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int limit = 6)
        {
            var query = new GetAllGuidelinesQuery
            {
                Page = page,
                Limit = limit
            };

            var userId = _authHelper.GetCurrentUserId(User);
            if (userId != Guid.Empty)
            {
                query.CurrentUserId = userId;
            }
            query.CurrentRole = _authHelper.GetCurrentRole(User);

            var result = await _mediator.Send(query);
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

        public class AssignGuidelineRequest
        {
            public Guid GuidelineId { get; set; }
            public Guid ChildId { get; set; }
        }

        // POST: api/Guidelines/assign — Specialist only
        [HttpPost("assign")]
        [Authorize(Roles = "specialist")]
        public async Task<IActionResult> Assign([FromBody] AssignGuidelineRequest request)
        {
            var currentUserId = _authHelper.GetCurrentUserId(User);
            var command = new AssignGuidelineCommand
            {
                GuidelineId = request.GuidelineId,
                ChildId = request.ChildId,
                AssignedBy = currentUserId
            };

            var result = await _mediator.Send(command);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, new
                {
                    status = "error",
                    code = result.StatusCode,
                    message = result.ErrorMessage,
                    timestamp = DateTime.UtcNow.ToString("o")
                });
            }

            return Ok(new { message = "Guideline assigned successfully" });
        }

        // GET: api/Guidelines/assigned/{childId} — Specialist or Parent with child validation
        [HttpGet("assigned/{childId:guid}")]
        [Authorize(Roles = "specialist,parent")]
        public async Task<IActionResult> GetAssigned(Guid childId)
        {
            var role = _authHelper.GetCurrentRole(User);
            if (string.Equals(role, "parent", StringComparison.OrdinalIgnoreCase))
            {
                var parentId = _authHelper.GetCurrentUserId(User);
                if (parentId == Guid.Empty)
                {
                    return Unauthorized(new
                    {
                        status = "error",
                        code = 401,
                        message = "Invalid token.",
                        timestamp = DateTime.UtcNow.ToString("o")
                    });
                }

                var isAuthorized = await _authHelper.IsParentAuthorizedForChild(childId, parentId);
                if (!isAuthorized)
                {
                    return StatusCode(403, new
                    {
                        status = "error",
                        code = 403,
                        message = "Access denied — you do not have permission to access this child's data",
                        timestamp = DateTime.UtcNow.ToString("o")
                    });
                }
            }

            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var result = await _mediator.Send(new GetAssignedGuidelinesQuery(childId, baseUrl));
            return Ok(result);
        }
    }
}
