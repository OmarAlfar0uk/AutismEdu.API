using AutismEdu.API.Contracts;
using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AutismEdu.API.Repositories
{
    public class JwtService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;

        public JwtService(IConfiguration config, UserManager<ApplicationUser> userManager)
        {
            _config = config;
            _userManager = userManager;
        }

        public async Task<(string AccessToken, string RefreshToken)> GenerateTokensAsync(ApplicationUser user, bool rememberMe)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var userId = user.Id.ToString();
            var email = user.Email ?? string.Empty;

            var claims = new List<Claim>
            {
                new Claim("id", userId),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(ClaimTypes.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var accessToken = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );
            var accessTokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);

            var refreshToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var refreshTokenExpiry = rememberMe ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddDays(7);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = refreshTokenExpiry;
            await _userManager.UpdateAsync(user);

            return (accessTokenString, refreshToken);
        }

        public async Task<string?> RefreshAccessTokenAsync(string refreshToken)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.RefreshToken == refreshToken);
            if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
                return null;

            var roles = await _userManager.GetRolesAsync(user);
            return GenerateAccessToken(user, roles);
        }

        private string GenerateAccessToken(ApplicationUser user, IList<string> roles)
        {
            var userId = user.Id.ToString();
            var email = user.Email ?? string.Empty;

            var claims = new List<Claim>
            {
                new Claim("id", userId),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(ClaimTypes.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task RevokeRefreshTokenAsync(ApplicationUser user)
        {
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;
            await _userManager.UpdateAsync(user);
        }

    }
}
