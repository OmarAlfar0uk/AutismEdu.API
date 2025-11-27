using AutismEdu.API.Models;

namespace AutismEdu.API.Contracts
{
    public interface ITokenService
    {
        Task<(string AccessToken, string RefreshToken)> GenerateTokensAsync(ApplicationUser user, bool rememberMe);
        Task<string?> RefreshAccessTokenAsync(string refreshToken);

        Task RevokeRefreshTokenAsync(ApplicationUser user);
    }
}
