using AutismEdu.API.Features.Auth.Login;
using MediatR;

namespace AutismEdu.API.Features.Auth.GetCurrentUser
{
    public record GetCurrentUserQuery(string UserId) : IRequest<LoginResponse>;
}
