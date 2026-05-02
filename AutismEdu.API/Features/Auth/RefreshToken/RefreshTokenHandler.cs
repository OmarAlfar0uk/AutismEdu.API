using AutismEdu.API.Contracts;
using MediatR;

namespace AutismEdu.API.Features.Auth.RefreshToken
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
    {
        private readonly ITokenService _tokenService;

        public RefreshTokenHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var newAccessToken = await _tokenService.RefreshAccessTokenAsync(request.RefreshToken);

            if (newAccessToken == null)
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");

            return new RefreshTokenResponse
            {
                AccessToken = newAccessToken,
                ExpiresIn = 1800 // 30 minutes in seconds (matches JwtService config)
            };
        }
    }
}
