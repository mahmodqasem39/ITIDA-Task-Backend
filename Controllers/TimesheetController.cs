using ITIDATask.Services;
using AutoMapper;
using ITIDATask.Utitlites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ITIDATask.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Diagnostics;
using System;

namespace ITIDATask.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class TimesheetController : ControllerBase
    {
        private readonly ITimesheetService _timesheetservice;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountController> _logger;


        public TimesheetController(ITimesheetService timesheetservice, IMapper mapper, ILogger<AccountController> logger)
        {
            _timesheetservice = timesheetservice;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(SubmitTimeModel timeModel) 
        {
            try
            {
                var timesheet = _mapper.Map<Timesheet>(timeModel);
                _logger.LogInformation("Submit Registerd Time Function Attemped ..");
                _logger.LogDebug($"SubmitRegisterdTime Params is Date : {timeModel.RegisterDate} , " +
                    $"loginTime : {timeModel.LoginTime} , LogoutTime : {timeModel.LogoutTime} , User : {timeModel.UserID}" );
                var result = await _timesheetservice.SubmitRegisterdTime(timesheet);
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

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(string userId)
        {
            try
            {
                _logger.LogInformation("GetAll Submited Time Function Attemped ..");
#if DEBUG
_logger.LogDebug("This is a debug message");
#endif
                _logger.LogDebug($"GetAll Registerd Timm  Params is user : {userId} ");
                 var result = await _timesheetservice.GetAllRegiterdTime(userId);
                if (result.Success == true)
                {
                    return Ok(result.Payload);
                }
                return BadRequest(result.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Register");
                throw;
            }
        }


        [HttpPut("Update")]
        public async Task<IActionResult> Update(UpdateSubmitedTimetModel model)
        {
            try
            {
                _logger.LogInformation("Updated Submited Time Function Attemped ..");
                _logger.LogDebug($"Updated Submited Time  Params is user : {model.UserID}, LoginTime :{model.LoginTime} , LogoutTime :{model.LogoutTime} ");
                var result = await _timesheetservice.UpdateSubmitedTime(model);
                if (result.Success == true)
                {
                    return Ok(result.Message);
                }
                return BadRequest(result.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during update time");
                throw;
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation("Delete Submited TimeFunction Attemped ..");
                _logger.LogDebug($"Updated Submited Time  Params is id: {id}"); 
                var result = await _timesheetservice.DeleteSubmitedTime(id);
                if (result.Success == true)
                {
                    return Ok(result.Message);
                }
                return BadRequest(result.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during update time");
                throw;
            }
        }
    }
}
