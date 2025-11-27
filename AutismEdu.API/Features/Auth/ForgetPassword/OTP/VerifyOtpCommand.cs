using MediatR;

namespace AutismEdu.API.Features.Auth.ForgetPassword.OTP
{
    public record VerifyOtpCommand(string Email, string OtpCode) : IRequest<bool>;

}
