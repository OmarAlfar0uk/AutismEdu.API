using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AutismEdu.API.Features.Auth.Authorization
{
    public interface IAuthorizationHelper
    {
        Task<bool> IsParentAuthorizedForChild(Guid childId, Guid parentId);
        Guid GetCurrentUserId(ClaimsPrincipal user);
        string GetCurrentRole(ClaimsPrincipal user);
    }
}
