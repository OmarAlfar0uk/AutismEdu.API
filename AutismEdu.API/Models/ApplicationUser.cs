using Microsoft.AspNetCore.Identity;

namespace AutismEdu.API.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string? ProfileImageUrl { get; set; }
       
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string FullName { get; set; }

       
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public ICollection<ChildProfile>? ChildProfiles { get; set; }

    }
}
