using AutismEdu.API.Features.Patients.Create;
using AutismEdu.API.Features.Patients.GetAll;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AutismEdu.API.Features.Patients
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PatientsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Lists patients. Specialists can view all patients and filter by parent email using search; parents receive only their own children from the JWT user id.
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "specialist,parent")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllPatientsQuery query)
        {
            query.IsSpecialist = User.IsInRole("specialist");

            if (User.IsInRole("parent"))
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? User.FindFirstValue("id");
                if (!Guid.TryParse(userIdClaim, out var parentId))
                    return NotFound(Error(404, "Child not found or not accessible"));

                query.ParentUserId = parentId;
                query.ParentEmail = User.FindFirstValue(ClaimTypes.Email)
                    ?? User.FindFirstValue(JwtRegisteredClaimNames.Email)
                    ?? User.FindFirstValue("email");
            }

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "specialist,Admin")]
        public async Task<IActionResult> Create([FromBody] CreatePatientCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(201, result);
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
