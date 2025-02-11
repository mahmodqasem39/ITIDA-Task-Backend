using ITIDATask.DAL.Entities;
using ITIDATask.Utitlites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ITIDATask.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        AppSettings _appSettings;


        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptionsMonitor<AppSettings> settings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = settings.CurrentValue;
        }

        /// <inheritdoc/>
        public async Task<OperationResult> RegisterAsync(RegisterModel registerModel)
        {

            var user = new ApplicationUser
            {
                UserName = registerModel.Email,
                Email = registerModel.Email,
                PhoneNumber = registerModel.MobileNumber,
                Name = registerModel.Name
                
            };

            if (await _userManager.FindByEmailAsync(registerModel.Email) != null)
            {
                return OperationResult.Existed("user already exisit");
            }

            var result = await _userManager.CreateAsync(user,registerModel.Password);

            if (result.Succeeded)
            {
                return OperationResult.Succeeded();
            }

            return OperationResult.Failed(payload:user);
        }

        /// <inheritdoc/>
        public async Task<OperationResult> ValidateUserAsync(LoginModel loginModel)
        {
            var normalizedUserName = _userManager.NormalizeName(loginModel.Email);
            var existedUser = await _userManager.Users
                .FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName || u.NormalizedEmail == normalizedUserName || u.PhoneNumber == normalizedUserName);

            if (existedUser != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(existedUser, loginModel.Password, true);

                if (result.Succeeded)
                {
                    var user = new UserViewModel
                    {
                        Email = existedUser.Email,
                        Mobile = existedUser.PhoneNumber,
                        Id = existedUser.Id,
                        Name = existedUser.Name
                    };

                    var claims = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("userId", user.Id),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    });

                    var token = await GetAccessTokenAsync(claims);

                    var loginResponse = new LoginResponse
                    {
                        token = token.AccessToken,
                        ExpiresIn = token.Expires,
                        //User = user
                    };

                    return OperationResult.Succeeded(payload: loginResponse);
                }
                return OperationResult.Failed(msg: "Invalid Password", payload: result);
            }
            return OperationResult.Failed("Invalid User!.");
        }

        public async Task<TokenModel> GetAccessTokenAsync(ClaimsIdentity claims)
        {
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddDays(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var jwtHandler = new JwtSecurityTokenHandler();

            var token = jwtHandler.CreateToken(tokenDescriptor);
            var access_token = jwtHandler.WriteToken(token);
            var userId = claims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            return new TokenModel
            {
                AccessToken = access_token,
                Expires = token.ValidTo,
            };
        }

        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }

    }
}

