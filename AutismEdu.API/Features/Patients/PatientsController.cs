using AutismEdu.API.Features.Patients.Create;
using AutismEdu.API.Features.Patients.GetAll;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutismEdu.API.Features.Patients
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "specialist,Admin")] // Specialist/Admin roles for the whole controller
    public class PatientsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PatientsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllPatientsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePatientCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(201, result);
        }
    }
}
