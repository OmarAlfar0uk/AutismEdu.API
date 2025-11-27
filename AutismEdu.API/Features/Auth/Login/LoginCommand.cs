using MediatR;

namespace AutismEdu.API.Features.Auth.Login
{
    public record LoginCommand(string Email, string Password, bool RememberMe) : IRequest<LoginResponse>;

}
