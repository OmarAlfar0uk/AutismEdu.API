namespace AutismEdu.API.Features.Auth.UpdateUserProfile
{
    public record UpdateUserProfileRequest
  (
      string? FirstName,
      string? LastName,
      string? PhoneNumber,
      IFormFile? ProfileImage
  );
}
