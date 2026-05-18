using System.Security.Claims;

namespace AutismEdu.API.Features.Auth.Authorization
{
    public interface IChildAuthorizationService
    {
        bool IsAuthorizedForChild(Guid childId, ClaimsPrincipal user);
    }
}
