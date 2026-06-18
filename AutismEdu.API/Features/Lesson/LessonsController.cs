using AutismEdu.API.Features.Lesson.Add;
using AutismEdu.API.Features.Lesson.ChildLevel;
using AutismEdu.API.Features.Lesson.Delete;
using AutismEdu.API.Features.Lesson.GenerateLesso;
using AutismEdu.API.Features.Lesson.GetAll;
using AutismEdu.API.Features.Lesson.GetById;
using AutismEdu.API.Features.Lesson.Update;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AutismEdu.API.Features.Lesson
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonsController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly Features.Auth.Authorization.IAuthorizationHelper _authHelper;

        public LessonsController(IMediator mediator, Features.Auth.Authorization.IAuthorizationHelper authHelper)
        {
            _mediator = mediator;
            _authHelper = authHelper;
        }
        //[HttpPost("{id}/tts")]
        //public async Task<IActionResult> GenerateSpeech(Guid id)
        //{
        //    var audioUrl = await _mediator.Send(new GenerateLessonSpeechCommand { LessonId = id });

        //    return Ok(new
        //    {
        //        success = true,
        //        audioUrl = $"{Request.Scheme}://{Request.Host}{audioUrl}"
        //    });
        //}




        // GET: api/Lessons (with data isolation)
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllLessonsQuery();

            query.CurrentUserId = _authHelper.GetCurrentUserId(User);
            query.CurrentRole = _authHelper.GetCurrentRole(User);

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // GET: api/Lessons/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetLessonByIdQuery { Id = id });

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        // POST: api/Lessons
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLessonCommand command)
        {
            var id = await _mediator.Send(command);

            return Ok(new
            {
                success = true,
                lessonId = id
            });
        }

        // PUT: api/Lessons/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLessonCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id mismatch");

            await _mediator.Send(command);

            return Ok(new { success = true });
        }

        // DELETE: api/Lessons/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteLessonCommand { Id = id });

            return Ok(new { success = true });
        }

        // POST: api/Lessons/{id}/tts
        [HttpPost("{id}/tts")]
        public async Task<IActionResult> GenerateSpeech(Guid id)
        {
            var audioUrl = await _mediator.Send(new GenerateLessonSpeechCommand
            {
                LessonId = id
            });

            var fullUrl = $"{Request.Scheme}://{Request.Host}{audioUrl}";

            return Ok(new
            {
                success = true,
                audioUrl = fullUrl
            });
        }

        // GET: api/Lessons/{id}/child-level — Parent only
        [HttpGet("{id:guid}/child-level")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> GetChildLevel(Guid id, [FromQuery] Guid? child_id)
        {
            // Infer child_id from parent's token or use query param
            var childId = child_id;
            if (!childId.HasValue)
            {
                // Try to get from claims
                var userIdClaim = User.FindFirst("id")?.Value;
                // For now, child_id must be passed as query param
            }

            var result = await _mediator.Send(new GetChildLevelQuery
            {
                LessonId = id,
                ChildId = childId
            });

            if (result == null)
                return NotFound(new { status = "error", code = 404, message = "No level recorded yet.", timestamp = DateTime.UtcNow });

            return Ok(result);
        }

        // PUT: api/Lessons/{id}/child-level — Specialist only
        [HttpPut("{id:guid}/child-level")]
        [Authorize(Roles = "specialist")]
        public async Task<IActionResult> UpsertChildLevel(Guid id, [FromBody] UpsertChildLevelCommand command)
        {
            command.LessonId = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }

}
