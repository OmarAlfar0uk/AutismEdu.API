using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutismEdu.API.Contracts;
using AutismEdu.API.Models;

namespace AutismEdu.API.Features.Auth.Authorization
{
    public class AuthorizationHelper : IAuthorizationHelper
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthorizationHelper(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> IsParentAuthorizedForChild(Guid childId, Guid parentId)
        {
            var repo = _unitOfWork.GetRepository<ChildProfile>();
            var child = await repo.GetByIdAsync(childId);
            return child != null && child.UserId == parentId;
        }

        public Guid GetCurrentUserId(ClaimsPrincipal user)
        {
            var userIdStr = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue("id");
            if (Guid.TryParse(userIdStr, out var userId))
            {
                return userId;
            }
            return Guid.Empty;
        }

        public string GetCurrentRole(ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
        }
    }
}
