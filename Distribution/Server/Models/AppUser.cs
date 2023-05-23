using Microsoft.AspNetCore.Identity;

namespace Distribution.Models
{
    public class AppUser : IdentityUser<string>
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string? FirstName { get; set; }

        public string? LastName { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }

        public AppUser()
        {
        }
    }
}
