using MediatR;

namespace AutismEdu.API.Features.Auth.Register
{

    public record RegisterCommand(RegisterDto RegisterDto) : IRequest<RegisterResponse>;

}
