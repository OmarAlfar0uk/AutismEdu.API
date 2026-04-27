using AutismEdu.API.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutismEdu.API.Features.CommunicationCard
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChildCardsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChildCardsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Get all cards assigned to a child
        [HttpGet("{childId:guid}")]
        public async Task<IActionResult> GetChildCards(Guid childId)
        {
            var result = await _mediator.Send(new GetChildCardsQuery(childId));
            return Ok(result);
        }
    }
}
