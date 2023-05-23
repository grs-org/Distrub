using Microsoft.AspNetCore.Identity;

namespace Distribution.Models
{
    public class AppRole : IdentityRole<string>
    {
        public AppRole() : base()
            { }
        public AppRole(string roleName) : base(roleName)
        {
        }
    }
}
