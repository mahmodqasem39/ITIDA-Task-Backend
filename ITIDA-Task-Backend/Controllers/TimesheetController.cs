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
using log4net;

namespace ITIDATask.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class TimesheetController : ControllerBase
    {
        private readonly ITimesheetService _timesheetservice;
        private readonly IMapper _mapper;
        private readonly ILog _logger;


        public TimesheetController(ITimesheetService timesheetservice, IMapper mapper, ILog logger)
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
                _logger.Info("Submit Registerd Time Function Attemped ..");
                _logger.Debug($"SubmitRegisterdTime Params is Date : {timeModel.RegisterDate} , " +
                    $"loginTime : {timeModel.LoginTime} , LogoutTime : {timeModel.LogoutTime} , User : {timeModel.UserID}" );
                var result = await _timesheetservice.SubmitRegisterdTime(timesheet);
                if (result.Success == true)
                {
                    return Ok(result);
                }
                return BadRequest(new { errors = new[] { result.Message } });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return StatusCode(500, new { errors = new[] { "An internal server error occurred." } });
            }

        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(string userId)
        {
            try
            {
                _logger.Info("GetAll Submited Time Function Attemped ..");
                _logger.Debug($"GetAll Registerd Timm  Params is user : {userId} ");
                 var result = await _timesheetservice.GetAllRegiterdTime(userId);
                if (result.Success == true)
                {
                    return Ok(result.Payload);
                }
                return BadRequest(new { errors = new[] { result.Message } });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return StatusCode(500, new { errors = new[] { "An internal server error occurred." } });
            }
        }


        [HttpPut("Update")]
        public async Task<IActionResult> Update(UpdateSubmitedTimetModel model)
        {
            try
            {
                _logger.Info("Updated Submited Time Function Attemped ..");
                _logger.Debug($"Updated Submited Time  Params is user : {model.UserID}, LoginTime :{model.LoginTime} , LogoutTime :{model.LogoutTime} ");
                var result = await _timesheetservice.UpdateSubmitedTime(model);
                if (result.Success == true)
                {
                    return Ok(result.Message);
                }
                return StatusCode(500, new { errors = new[] { "An internal server error occurred." } });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.Info("Delete Submited TimeFunction Attemped ..");
                _logger.Debug($"Updated Submited Time  Params is id: {id}"); 
                var result = await _timesheetservice.DeleteSubmitedTime(id);
                if (result.Success == true)
                {
                    return Ok(result.Message);
                }
                return StatusCode(500, new { errors = new[] { "An internal server error occurred." } });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }
    }
}
