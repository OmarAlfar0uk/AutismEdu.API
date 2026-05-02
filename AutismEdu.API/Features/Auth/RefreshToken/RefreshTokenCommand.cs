using MediatR;

namespace AutismEdu.API.Features.Auth.RefreshToken
{
    public class RefreshTokenCommand : IRequest<RefreshTokenResponse>
    {
        public string RefreshToken { get; set; } = default!;
    }

    public class RefreshTokenResponse
    {
        public string AccessToken { get; set; } = default!;
        public int ExpiresIn { get; set; }
    }
}
