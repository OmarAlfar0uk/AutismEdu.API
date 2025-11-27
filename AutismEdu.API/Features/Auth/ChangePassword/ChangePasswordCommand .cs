using MediatR;

namespace AutismEdu.API.Features.Auth.ChangePassword
{
    public record ChangePasswordCommand(
       string CurrentPassword,
       string NewPassword
   ) : IRequest<bool>;
}
