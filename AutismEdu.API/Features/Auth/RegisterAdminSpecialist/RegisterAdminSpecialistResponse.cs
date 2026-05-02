namespace AutismEdu.API.Features.Auth.RegisterAdminSpecialist
{
    public record RegisterAdminSpecialistResponse(
         bool Success,
         string Message,
         Guid UserId,
         string UserName,
         string FirstName,
         string LastName,
         string FullName,
         string PhoneNumber,
         string Email,
         string ProfileImageUrl,
         IList<string> Roles,
         string Token,
         string? RefreshToken
        );
}
