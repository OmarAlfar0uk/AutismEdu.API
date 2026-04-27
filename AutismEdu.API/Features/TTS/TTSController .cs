using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutismEdu.API.Features.TTS
{
    [Route("api/[controller]")]
    [ApiController]
    public class TTSController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TTSController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Generate([FromBody] GenerateTTSCommand command)
        {
            var url = await _mediator.Send(command);

            var fullUrl = $"{Request.Scheme}://{Request.Host}{url}";

            return Ok(new
            {
                success = true,
                audioUrl = fullUrl
            });
        }
    }
}
