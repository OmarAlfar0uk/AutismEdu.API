using AutismEdu.API.Features.CommunicationCard.AssignCardToChild;
using AutismEdu.API.Features.CommunicationCard.Create;
using AutismEdu.API.Features.CommunicationCard.Delete;
using AutismEdu.API.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AutismEdu.API.Features.CommunicationCard
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunicationCardController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly Features.Auth.Authorization.IAuthorizationHelper _authHelper;

        public CommunicationCardController(IMediator mediator, Features.Auth.Authorization.IAuthorizationHelper authHelper)
        {
            _mediator = mediator;
            _authHelper = authHelper;
        }

        // ---------------------------------------------
        // POST: Create Card
        // ---------------------------------------------
        [HttpPost("create")]
        [Authorize(Roles = "specialist,Admin")]
        public async Task<IActionResult> Create([FromForm] CreateCommunicationCardCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(new { Id = id });
        }

        // ---------------------------------------------
        // GET: Get All Cards (with data isolation)
        // ---------------------------------------------
        [HttpGet("all")]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllCommunicationCardsQuery();

            query.CurrentUserId = _authHelper.GetCurrentUserId(User);
            query.CurrentRole = _authHelper.GetCurrentRole(User);

            var cards = await _mediator.Send(query);
            return Ok(cards);
        }

        // ---------------------------------------------
        // GET: Get Card by ID
        // ---------------------------------------------
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var card = await _mediator.Send(new GetCommunicationCardByIdQuery(id));
            if (card == null)
                return NotFound();

            return Ok(card);
        }

        // ---------------------------------------------
        // GET: Get Cards by Category
        // ---------------------------------------------
        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetByCategory(string category)
        {
            var cards = await _mediator.Send(new GetCommunicationCardByCategoryQuery(category));
            return Ok(cards);
        }

        // ---------------------------------------------
        // DELETE: Delete Card
        // ---------------------------------------------
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteCommunicationCardCommand(id));

            if (!result)
                return NotFound();

            return Ok(new { Message = "Deleted successfully" });
        }

        // ---------------------------------------------
        // POST: Assign Card To Child
        // ---------------------------------------------
        [HttpPost("assign")]
        public async Task<IActionResult> AssignToChild([FromBody] AssignCardToChildCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new { Assigned = result });
        }
    }
}
