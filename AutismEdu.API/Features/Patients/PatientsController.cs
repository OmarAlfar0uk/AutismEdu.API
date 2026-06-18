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
        private readonly Features.Auth.Authorization.IAuthorizationHelper _authHelper;

        public PatientsController(IMediator mediator, Features.Auth.Authorization.IAuthorizationHelper authHelper)
        {
            _mediator = mediator;
            _authHelper = authHelper;
        }

        /// <summary>
        /// Lists patients. Specialists can view all patients and filter by parent email using search; parents receive only their own children from the JWT user id.
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "specialist,parent")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllPatientsQuery query)
        {
            var role = _authHelper.GetCurrentRole(User);
            var currentUserId = _authHelper.GetCurrentUserId(User);

            query.IsSpecialist = string.Equals(role, "specialist", StringComparison.OrdinalIgnoreCase);

            if (query.IsSpecialist)
            {
                query.SpecialistUserId = currentUserId;
            }

            if (string.Equals(role, "parent", StringComparison.OrdinalIgnoreCase))
            {
                if (currentUserId == Guid.Empty)
                    return NotFound(Error(404, "Child not found or not accessible"));

                query.ParentUserId = currentUserId;
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
