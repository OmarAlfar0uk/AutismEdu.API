using AutismEdu.API.Features.ChatBot.AskQuestion;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutismEdu.API.Features.ChatBot
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatBotController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChatBotController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] AskQuestionCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
