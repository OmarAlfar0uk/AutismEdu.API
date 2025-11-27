using MediatR;

namespace AutismEdu.API.Features.Auth.ForgetPassword.OTP
{
    public record SendOtpCommand(string Email) : IRequest<bool>;
}
