using AutismEdu.API.Features.CommunicationCard.AssignCardToChild;
using AutismEdu.API.Features.CommunicationCard.Create;
using AutismEdu.API.Features.CommunicationCard.Delete;
using AutismEdu.API.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutismEdu.API.Features.CommunicationCard
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunicationCardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CommunicationCardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ---------------------------------------------
        // POST: Create Card
        // ---------------------------------------------
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CreateCommunicationCardCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(new { Id = id });
        }

        // ---------------------------------------------
        // GET: Get All Cards
        // ---------------------------------------------
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var cards = await _mediator.Send(new GetAllCommunicationCardsQuery());
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
