using AutismEdu.API.Features.Performance.Add;
using AutismEdu.API.Features.Auth.Authorization;
using AutismEdu.API.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutismEdu.API.Features.Performance
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerformanceController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IChildAuthorizationService _childAuthorizationService;

        public PerformanceController(IMediator mediator, IChildAuthorizationService childAuthorizationService)
        {
            _mediator = mediator;
            _childAuthorizationService = childAuthorizationService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddPerformanceCommand command)
        {
            var recordId = await _mediator.Send(command);

            return Ok(new
            {
                success = true,
                recordId
            });
        }
        /// <summary>
        /// Gets performance for a child. Specialists may access any child; parents may access only children owned by their JWT user id and otherwise receive 404.
        /// </summary>
        [HttpGet("child/{childId}")]
        [Authorize(Roles = "specialist,parent")]
        public async Task<IActionResult> GetChildPerformance(Guid childId)
        {
            if (!_childAuthorizationService.IsAuthorizedForChild(childId, User))
                return NotFound(Error(404, "Child not found or not accessible"));

            var result = await _mediator.Send(new GetChildPerformanceQuery { ChildId = childId });
            return Ok(result);
        }

        /// <summary>
        /// Gets lesson performance for a child. Specialists may access any child; parents may access only children owned by their JWT user id and otherwise receive 404.
        /// </summary>
        [HttpGet("child/{childId}/lesson/{lessonId}")]
        [Authorize(Roles = "specialist,parent")]
        public async Task<IActionResult> GetLessonPerformance(Guid childId, Guid lessonId)
        {
            if (!_childAuthorizationService.IsAuthorizedForChild(childId, User))
                return NotFound(Error(404, "Child not found or not accessible"));

            var result = await _mediator.Send(new GetLessonPerformanceQuery
            {
                ChildId = childId,
                LessonId = lessonId
            });

            if (result is null)
                return NotFound(Error(404, "Performance not found."));

            return Ok(result);
        }

        /// <summary>
        /// Gets performance stats for a child. Specialists may access any child; parents may access only children owned by their JWT user id and otherwise receive 404.
        /// </summary>
        [HttpGet("child/{childId}/stats")]
        [Authorize(Roles = "specialist,parent")]
        public async Task<IActionResult> GetStats(Guid childId)
        {
            if (!_childAuthorizationService.IsAuthorizedForChild(childId, User))
                return NotFound(Error(404, "Child not found or not accessible"));

            var result = await _mediator.Send(new GetChildPerformanceStatsQuery
            {
                ChildId = childId
            });

            return Ok(result);
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
