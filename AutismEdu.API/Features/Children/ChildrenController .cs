using AutismEdu.API.Features.Children.Add;
using AutismEdu.API.Features.Children.Delete;
using AutismEdu.API.Features.Children.Update;
using AutismEdu.API.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutismEdu.API.Features.Children
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChildrenController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChildrenController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateChildCommand command)
        {
            var id = await _mediator.Send(command);

            return Ok(new
            {
                success = true,
                childId = id
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateChildCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id mismatch");

            await _mediator.Send(command);

            return Ok(new { success = true });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteChildCommand { Id = id });

            return Ok(new { success = true });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetChildByIdQuery { Id = id });

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("by-parent/{userId}")]
        public async Task<IActionResult> GetByParent(Guid userId)
        {
            var result = await _mediator.Send(new GetChildrenByParentQuery { UserId = userId });
            return Ok(result);
        }
    }
}
