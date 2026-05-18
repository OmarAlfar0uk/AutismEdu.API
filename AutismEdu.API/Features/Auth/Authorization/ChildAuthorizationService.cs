using System.Security.Claims;
using AutismEdu.API.Contracts;
using AutismEdu.API.Models;

namespace AutismEdu.API.Features.Auth.Authorization
{
    public class ChildAuthorizationService : IChildAuthorizationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChildAuthorizationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool IsAuthorizedForChild(Guid childId, ClaimsPrincipal user)
        {
            if (user.IsInRole("specialist"))
                return true;

            if (!user.IsInRole("parent"))
                return false;

            var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out var parentId))
                return false;

            return _unitOfWork.GetRepository<ChildProfile>()
                .GetAllAsync()
                .Any(child => child.Id == childId && child.UserId == parentId);
        }
    }
}
