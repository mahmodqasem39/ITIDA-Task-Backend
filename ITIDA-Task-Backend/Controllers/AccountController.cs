using ITIDATask.Services;
using ITIDATask.Utitlites;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ITIDATask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;
        public AccountController(IAccountService accountService, ILogger<AccountController> logger) 
        {
            _accountService = accountService;
            _logger = logger;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            try
            {
                _logger.LogInformation($"Register Function Attempetd");
                _logger.LogDebug($"Register Params is , Email :{model.Email}");
                var result = await _accountService.RegisterAsync(model);
                if (result.Success == true)
                {
                    return Ok(result);
                }

                return BadRequest(result.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Register");
                throw;
            }
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                _logger.LogInformation($"Login Function Attempetd");
                _logger.LogDebug($"Login user  Params is , Email :{model.Email}");
                var result = await _accountService.ValidateUserAsync(model);
                if (result.Success == true)
                {
                    return Ok(result);
                }

                return BadRequest(result.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Register");
                throw;
            }
        }

        [HttpPost("Logout")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> LogOut()
        {
            await _accountService.SignOut();
            return Ok("sigend out succssfully");
        }
    }
}
