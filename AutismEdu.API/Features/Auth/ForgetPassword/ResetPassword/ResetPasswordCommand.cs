using MediatR;

namespace AutismEdu.API.Features.Auth.ForgetPassword.ResetPassword
{
    public record ResetPasswordCommand(string Email, string NewPassword) : IRequest<bool>;
}
