using AutismEdu.API.Features.Activities.Create;
using AutismEdu.API.Features.Activities.Delete;
using AutismEdu.API.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutismEdu.API.Features.Activities
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ActivitiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllActivitiesQuery());
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateActivityCommand command)
        {
            var id = await _mediator.Send(command);

            return Ok(new
            {
                success = true,
                activityId = id
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteActivityCommand { Id = id });
            return Ok(new { success = true });
        }
    }
}
