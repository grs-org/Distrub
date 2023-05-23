using Distribution.Models;
using Distribution.Shared.Constant;
using Distribution.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Distribution.Shared.Requests;
using Mapster;
using Distribution.Server.Services;

namespace Distribution.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServicecs _userService;
       


        public UserController(
          IUserServicecs userServices
         
          )
        {
            _userService = userServices;
        }

        [HttpGet]
        public async Task<IEnumerable<UserLoginDto>> GetAllAsync()
        {
            var users = await _userService.GetAllAsync();
            return users;
        }
        //public async Task<UserLoginDto> GetAsync(string userId)
        //{
        //    var user = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
        //    var result = user.Adapt<UserLoginDto>();
        //    return result;
        //}

        [HttpPost]
        public async Task<string> Register(RegisterRequest request)
        {
            var result = await _userService.Register(request);
            return result;
        }

        [HttpPut]
        public async Task<string> UpdateUser(UserLoginDto request)
        {
            var result = await _userService.UpdateUser(request);
            return result;
            
        }

        //public async Task<UserRolesResponse> GetRolesAsync(string userId)
        //{
        //    var viewModel = new List<UserRoleModel>();
        //    var user = await _userManager.FindByIdAsync(userId);
        //    var roles = await _roleManager.Roles.ToListAsync();

        //    foreach (var role in roles)
        //    {
        //        var userRolesViewModel = new UserRoleModel
        //        {
        //            RoleName = role.Name,
        //            RoleDescription = role.Description
        //        };
        //        if (await _userManager.IsInRoleAsync(user, role.Name))
        //        {
        //            userRolesViewModel.Selected = true;
        //        }
        //        else
        //        {
        //            userRolesViewModel.Selected = false;
        //        }

        //        viewModel.Add(userRolesViewModel);
        //    }

        //    var result = new UserRolesResponse { UserRoles = viewModel };
        //    return await Result<UserRolesResponse>.SuccessAsync(result);
        //}

        //public async Task<IResult> UpdateRolesAsync(UpdateUserRolesRequest request)
        //{
        //    var user = await _userManager.FindByIdAsync(request.UserId);
        //    if (user.Email == UserConstants.SuperAdminEmail)
        //    {
        //        return await Result.FailAsync(_localizer["Not Allowed."]);
        //    }

        //    var roles = await _userManager.GetRolesAsync(user);
        //    var selectedRoles = request.UserRoles.Where(x => x.Selected).ToList();

        //    var currentUser = await _userManager.FindByIdAsync(_currentUserService.UserId);
        //    if (!await _userManager.IsInRoleAsync(currentUser, RoleConstants.AdministratorRole))
        //    {
        //        var tryToAddAdministratorRole = selectedRoles
        //            .Any(x => x.RoleName == RoleConstants.AdministratorRole);
        //        var userHasAdministratorRole = roles.Any(x => x == RoleConstants.AdministratorRole);
        //        if (tryToAddAdministratorRole && !userHasAdministratorRole ||
        //            !tryToAddAdministratorRole && userHasAdministratorRole)
        //        {
        //            return await Result.FailAsync(
        //                _localizer["Not Allowed to add or delete Administrator Role if you have not this role."]);
        //        }
        //    }

        //    var result = await _userManager.RemoveFromRolesAsync(user, roles);
        //    result = await _userManager.AddToRolesAsync(user, selectedRoles.Select(y => y.RoleName));
        //    return await Result.SuccessAsync(_localizer["Roles Updated"]);
        //}

        //public async Task<string> ConfirmEmailAsync(string userId, string code)
        //{
        //    var user = await _userManager.FindByIdAsync(userId);
        //    code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        //    var result = await _userManager.ConfirmEmailAsync(user, code);
        //    if (result.Succeeded)
        //    {
        //        return "Account Confirmed for {0}. You can now use the /api/identity/token endpoint to generate JWT.";

        //    }
        //    else
        //    {
        //        throw new Exception(string.Format("An error occurred while confirming {0}"));
        //    }
        //}

        //public async Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
        //{
        //    var user = await _userManager.FindByEmailAsync(request.Email);
        //    if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
        //    {
        //        // Don't reveal that the user does not exist or is not confirmed
        //        return await Result.FailAsync(_localizer["An Error has occurred!"]);
        //    }

        //    // For more information on how to enable account confirmation and password reset please
        //    // visit https://go.microsoft.com/fwlink/?LinkID=532713
        //    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        //    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        //    var route = "account/reset-password";
        //    var endpointUri = new Uri(string.Concat($"{origin}/", route));
        //    var passwordResetURL = QueryHelpers.AddQueryString(endpointUri.ToString(), "Token", code);
        //    var mailcontent = _templateService.PasswordResetTemplate(passwordResetURL, user.FirstName);
        //    var mailRequest = new MailRequest
        //    {
        //        Body = mailcontent
        //        /*string.Format(_localizer["Please reset your passwordd by <a href='{0}>clicking here</a>."], HtmlEncoder.Default.Encode(passwordResetURL))*/,
        //        Subject = _localizer["Reset Password"],
        //        To = request.Email
        //    };
        //    BackgroundJob.Enqueue(() => _mailService.SendAsync(mailRequest));
        //    return await Result.SuccessAsync(_localizer["Password Reset Mail has been sent to your authorized Email."]);
        //}

        //public async Task<IResult> ResetPasswordAsync(ResetPasswordRequest request)
        //{
        //    var user = await _userManager.FindByEmailAsync(request.Email);
        //    if (user == null)
        //    {
        //        // Don't reveal that the user does not exist
        //        return await Result.FailAsync(_localizer["An Error has occured!"]);
        //    }

        //    var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
        //    if (result.Succeeded)
        //    {
        //        return await Result.SuccessAsync(_localizer["Password Reset Successful!"]);
        //    }
        //    else
        //    {
        //        return await Result.FailAsync(_localizer["An Error has occured!"]);
        //    }
        //}

    }
}
