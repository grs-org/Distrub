using Distribution.Models;
using Distribution.Shared;
using Distribution.Shared.Requests;
using Distribution.Shared.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Distribution.Server.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        //private readonly AppConfiguration _appConfig;


        public LoginController(
          UserManager<AppUser> userManager,
          RoleManager<AppRole> roleManager
          )
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<TokenResponse> LoginAsync(UserLoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordValid)
            {

                return new TokenResponse { Message = "mdp incorrecte" };
            }

            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _userManager.UpdateAsync(user);

            var token = await GenerateJwtAsync(user);
            var response = new TokenResponse
            { Token = token, RefreshToken = user.RefreshToken, Message = "Success" };
            return response;
        }

        [HttpPost("refresh")]
        public async Task<TokenResponse> GetRefreshTokenAsync(RefreshTokenRequest model)
        {
            if (model is null)
            {
                return new TokenResponse { Message = "Invalid Client Token." };
            }

            var userPrincipal = GetPrincipalFromExpiredToken(model.Token);
            var userEmail = userPrincipal.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
                return new TokenResponse { Message = "User Not Found." };
            if (user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return new TokenResponse { Message = "Invalid Client Token." };
            var token = GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user));
            user.RefreshToken = GenerateRefreshToken();
            await _userManager.UpdateAsync(user);

            var response = new TokenResponse
            {
                Token = token,
                RefreshToken = user.RefreshToken,
                RefreshTokenExpiryTime = user.RefreshTokenExpiryTime,
                Message = "Success"
            };
            return response;

        }

        private async Task<string> GenerateJwtAsync(AppUser user)
        {
            var token = GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user));
            return token;
        }

        private async Task<IEnumerable<Claim>> GetClaimsAsync(AppUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            var permissionClaims = new List<Claim>();
            foreach (var role in roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
                var thisRole = await _roleManager.FindByNameAsync(role);
                var allPermissionsForThisRoles = await _roleManager.GetClaimsAsync(thisRole);
                permissionClaims.AddRange(allPermissionsForThisRoles);
            }

            var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.Id),
                    new(ClaimTypes.Actor, user.UserName),
                    new(ClaimTypes.Email, user.Email),
                    new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty)
                }
                .Union(userClaims)
                .Union(roleClaims)
                .Union(permissionClaims);

            return claims;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
        {
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(2),
                signingCredentials: signingCredentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            var encryptedToken = tokenHandler.WriteToken(token);
            return encryptedToken;
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("5dqtETK72ZU@Gs6w!^hqJf9cgHz")),
                ValidateIssuer = false,
                ValidateAudience = false,
                RoleClaimType = ClaimTypes.Role,
                ClockSkew = TimeSpan.Zero
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var secret = Encoding.UTF8.GetBytes("5dqtETK72ZU@Gs6w!^hqJf9cgHz");
            return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
        }


    }
    }
