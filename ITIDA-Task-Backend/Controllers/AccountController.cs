using ITIDATask.Services;
using ITIDATask.Utitlites;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using log4net;

namespace ITIDATask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILog _logger;
        public AccountController(IAccountService accountService, ILog logger) 
        {
            _accountService = accountService;
            _logger = logger;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            try
            {
                _logger.Info($"Register Function Attempetd");
                _logger.Debug($"Register Params is , Email :{model.Email}");
                var result = await _accountService.RegisterAsync(model);
                if (result.Success == true)
                {
                    return Ok(result);
                }

                return BadRequest(result.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                _logger.Info($"Login Function Attempetd");
                _logger.Debug($"Login user  Params is , Email :{model.Email}");
                var result = await _accountService.ValidateUserAsync(model);
                if (result.Success == true)
                {
                    return Ok(result);
                }

                return BadRequest(result.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
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
