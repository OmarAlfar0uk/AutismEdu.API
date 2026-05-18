using System.Security.Claims;
using AutismEdu.API.Features.Children.Add;
using AutismEdu.API.Features.Children.Delete;
using AutismEdu.API.Features.Children.Update;
using AutismEdu.API.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Creates a child. Specialists may specify parentId/userId/emailParent; parent callers may omit them and are linked from the JWT user id.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "specialist,parent")]
        public async Task<IActionResult> Create([FromBody] CreateChildCommand command)
        {
            var result = await _mediator.Send(command);

            return StatusCode(201, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateChildCommand command)
        {
            if (id != command.Id)
                return BadRequest(Error(400, "Id mismatch"));

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
                return NotFound(Error(404, "Child not found or not accessible"));

            return Ok(result);
        }

        /// <summary>
        /// Gets children for a parent. Specialists may request any parent id; parent callers must request their own JWT user id or receive 404.
        /// </summary>
        [HttpGet("by-parent/{userId}")]
        [Authorize(Roles = "specialist,parent")]
        public async Task<IActionResult> GetByParent(Guid userId)
        {
            if (User.IsInRole("parent"))
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!Guid.TryParse(currentUserId, out var parentId) || parentId != userId)
                    return NotFound(Error(404, "Child not found or not accessible"));
            }

            var result = await _mediator.Send(new GetChildrenByParentQuery { UserId = userId });
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
