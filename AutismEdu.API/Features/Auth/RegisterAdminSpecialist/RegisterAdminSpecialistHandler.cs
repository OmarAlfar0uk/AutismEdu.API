using AutismEdu.API.Contracts;
using AutismEdu.API.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AutismEdu.API.Features.Auth.RegisterAdminSpecialist
{
    public class RegisterAdminSpecialistHandler : IRequestHandler<RegisterAdminSpecialistCommand, RegisterAdminSpecialistResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly ITokenService _tokenService;

        public RegisterAdminSpecialistHandler(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        public async Task<RegisterAdminSpecialistResponse> Handle(RegisterAdminSpecialistCommand request, CancellationToken cancellationToken)
        {
            var dto = request.RegisterDto;

            // 🔍 Check if user already exists
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new ApplicationException("Email already registered.");

            // 🧠 Create new ApplicationUser
            var user = new ApplicationUser
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                FullName = $"{dto.FirstName} {dto.LastName}",
                Email = dto.Email,
                UserName = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                EmailConfirmed = true,
                ProfileImageUrl = "/images/users/default-user.png",
                CreatedAt = DateTime.UtcNow
            };

            // 🔑 Create user in Identity
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new ApplicationException($"User creation failed: {errors}");
            }

            // 👤 Assign Admin and specialist roles
            if (await _roleManager.RoleExistsAsync("Admin"))
                await _userManager.AddToRoleAsync(user, "Admin");
            
            if (await _roleManager.RoleExistsAsync("specialist"))
                await _userManager.AddToRoleAsync(user, "specialist");

            // 🎭 Get roles
            var roles = await _userManager.GetRolesAsync(user);

            // ✅ Generate Access + Refresh Tokens
            bool rememberMe = dto.RememberMe;
            var (accessToken, refreshToken) = await _tokenService.GenerateTokensAsync(user, rememberMe);

            // 🧾 Return final response
            return new RegisterAdminSpecialistResponse(
                Success: true,
                Message: "Registration as Admin and Specialist successful",
                UserId: user.Id,
                UserName: user.UserName,
                FirstName: user.FirstName,
                LastName: user.LastName,
                FullName: user.FullName,
                Email: user.Email,
                PhoneNumber: user.PhoneNumber,
                ProfileImageUrl: user.ProfileImageUrl,
                Roles: roles,
                Token: accessToken,
                RefreshToken: refreshToken
            );
        }
    }
}
