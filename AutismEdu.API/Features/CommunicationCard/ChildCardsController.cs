using AutismEdu.API.Shared;
using AutismEdu.API.Features.Auth.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AutismEdu.API.Features.CommunicationCard
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChildCardsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAuthorizationHelper _authHelper;

        public ChildCardsController(IMediator mediator, IAuthorizationHelper authHelper)
        {
            _mediator = mediator;
            _authHelper = authHelper;
        }

        /// <summary>
        /// ISSUE 2: Get all cards assigned to a specific child.
        /// Parents can only access cards for their own children.
        /// Specialists can access cards for any child.
        /// </summary>
        [HttpGet("{childId:guid}")]
        [Authorize(Roles = "specialist,parent")]
        public async Task<IActionResult> GetChildCards(Guid childId)
        {
            var role = _authHelper.GetCurrentRole(User);

            if (string.Equals(role, "parent", StringComparison.OrdinalIgnoreCase))
            {
                var parentId = _authHelper.GetCurrentUserId(User);
                if (parentId == Guid.Empty)
                {
                    return Unauthorized(new
                    {
                        status = "error",
                        code = 401,
                        message = "Invalid token.",
                        timestamp = DateTime.UtcNow.ToString("o")
                    });
                }

                var isAuthorized = await _authHelper.IsParentAuthorizedForChild(childId, parentId);
                if (!isAuthorized)
                {
                    return StatusCode(403, new
                    {
                        status = "error",
                        code = 403,
                        message = "Access denied — you do not have permission to access this child's data",
                        timestamp = DateTime.UtcNow.ToString("o")
                    });
                }
            }

            var result = await _mediator.Send(new GetAssignedCardsForChildQuery(childId));
            return Ok(result);
        }
    }
}
