using Distribution.Models;
using Distribution.Shared;
using Distribution.Shared.Constant;
using Distribution.Shared.Requests;
using Distribution.Shared.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Distribution.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public AuthController(
          UserManager<AppUser> userManager,
          RoleManager<AppRole> roleManager
         )
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<string> Register(UserLoginDto request)
        {
            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                return  ("Username {0} is already taken.");
            }

            var user = new AppUser
            {

                Email = request.Email,
               
                UserName = request.UserName,
                PhoneNumber = "324242323",
                EmailConfirmed = true,
                
            };

          var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, AdminConstants.AdminRole);
                //if (!request.AutoConfirmEmail)
                //{
                //    var verificationUri = await SendVerificationEmail(user, origin);
                //    var mailRequest = new MailRequest
                //    {
                //        From = "boutamen@gmail.com",
                //        To = user.Email,
                //        Body = string.Format(
                //            _localizer["Please confirm your account by <a href='{0}'>clicking here</a>."],
                //            verificationUri),
                //        Subject = _localizer["Confirm Registration"]
                //    };
                //    BackgroundJob.Enqueue(() => _mailService.SendAsync(mailRequest));
                //    return await Result<string>.SuccessAsync(user.Id,
                //        string.Format(_localizer["User {0} Registered. Please check your Mailbox to verify!"],
                //            user.UserName));
                //}

                return "User {0} Registered.";
                   
                }
                else
                {
                return "Error";
                }
            }


        
    }
}
