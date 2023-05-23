using Distribution.Models;
using Distribution.Shared;
using Distribution.Shared.Constant;
using Distribution.Shared.Requests;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Distribution.Server.Services
{

    public interface IUserServicecs
    {
        Task<IEnumerable<UserLoginDto>> GetAllAsync();
        Task<string> Register(RegisterRequest request);
        Task<string> UpdateUser(UserLoginDto request);
    }
    public class UserServicecs : IUserServicecs
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;



        public UserServicecs(
          UserManager<AppUser> userManager,
          RoleManager<AppRole> roleManager

          )
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<UserLoginDto>> GetAllAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var result = users.Adapt<IEnumerable<UserLoginDto>>();
            return result;
        }
     
        public async Task<string> Register(RegisterRequest request)
        {
            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                return ("Username {0} is already taken.");
            }

            var user = new AppUser
            {

                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
                IsActive = request.ActivateUser,
                EmailConfirmed = request.AutoConfirmEmail

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

                return "Success";

            }
            else
            {
                return "Error";
            }
        }

        [HttpPut]
        public async Task<string> UpdateUser(UserLoginDto request)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            user.Email = request.Email;
            user.UserName = request.UserName;

            var response = await _userManager.UpdateAsync(user);
            if (response.Succeeded)
            {
                return "Success";
            }
            else
            {
                return "Failed";
            }
        }
    }
}
