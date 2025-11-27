using MediatR;

namespace AutismEdu.API.Features.Auth.Logout
{
    public record LogoutCommand(Guid UserId) : IRequest<LogoutResponse>;
}
