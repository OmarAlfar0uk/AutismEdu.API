using AutismEdu.API.Features.Performance.Add;
using AutismEdu.API.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutismEdu.API.Features.Performance
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerformanceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PerformanceController(IMediator mediator)
        {
            _mediator = mediator;
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
        // GET: api/performance/child/{childId}
        [HttpGet("child/{childId}")]
        public async Task<IActionResult> GetChildPerformance(Guid childId)
        {
            var result = await _mediator.Send(new GetChildPerformanceQuery { ChildId = childId });
            return Ok(result);
        }

        // GET: api/performance/child/{childId}/lesson/{lessonId}
        [HttpGet("child/{childId}/lesson/{lessonId}")]
        public async Task<IActionResult> GetLessonPerformance(Guid childId, Guid lessonId)
        {
            var result = await _mediator.Send(new GetLessonPerformanceQuery
            {
                ChildId = childId,
                LessonId = lessonId
            });

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        // GET: api/performance/child/{childId}/stats
        [HttpGet("child/{childId}/stats")]
        public async Task<IActionResult> GetStats(Guid childId)
        {
            var result = await _mediator.Send(new GetChildPerformanceStatsQuery
            {
                ChildId = childId
            });

            return Ok(result);
        }


    }
}
