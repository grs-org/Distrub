using System.ComponentModel.DataAnnotations;

namespace Distribution.Shared.Requests
{
public class RegisterRequest
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

  
    public string Email { get; set; }

    public string UserName { get; set; }
  
    public string Password { get; set; }
    //[Required]
    //[Compare(nameof(Password))]
    //public string ConfirmPassword { get; set; }

    public string PhoneNumber { get; set; }
    //public string SageId { get; set; }

    //[Required]
    //public string Role { get; set; }

    //[Required]
    //public string RoleType { get; set; }
    public bool ActivateUser { get; set; } = true;
    public bool AutoConfirmEmail { get; set; } = true;

    //public UpdateProfilePictureRequest ProfilePictureRequest { get; set; }
}


}
