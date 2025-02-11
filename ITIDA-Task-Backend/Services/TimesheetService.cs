using AutoMapper;
using ITIDATask.DAL.Entities;
using ITIDATask.Repositories.Interfaces;
using ITIDATask.Utitlites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ITIDATask.Services
{
    public class TimesheetService : ITimesheetService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public TimesheetService(IUnitOfWork unitOfWork,IMapper mapper,UserManager<ApplicationUser> userManage)
        {
           _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManage;
        }


        /// <inheritdoc/>
        public async Task<OperationResult> SubmitRegisterdTime(Timesheet submitModel)
        {
            var currentDate = DateOnly.FromDateTime(DateTime.Now); 
            if(DateOnly.FromDateTime(submitModel.RegisterDate) > currentDate)
            {
                return OperationResult.Failed("Date cannot be in the future");
            }

            if (submitModel.LoginTime >= submitModel.LogoutTime)
            {
                return OperationResult.Failed("Logout time must be after login time." );
            }

            var subimtedBefor = await _unitOfWork.GetRepository<Timesheet>()
                        .FindAsync(x => x.RegisterDate == submitModel.RegisterDate && x.UserId == submitModel.UserId);

            if (subimtedBefor.FirstOrDefault() == null)
            {
                var repo = _unitOfWork.GetRepository<Timesheet>();
                await repo.AddAsync(submitModel);
                await _unitOfWork.SaveChangesAsync();
                return OperationResult.Succeeded();
            }
            return OperationResult.Existed("This date Is submited befor for this user");
        }


        /// <inheritdoc/>
        public async Task<OperationResult> GetAllRegiterdTime(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var repository = _unitOfWork.GetRepository<Timesheet>();
                var timesheets = repository.FindAsync(x => x.UserId == userId).Result.OrderByDescending(x => x.RegisterDate);
                var timesheetsDto = _mapper.Map<IEnumerable<TimeSheetModel>>(timesheets);
                return OperationResult.Succeeded(payload: timesheetsDto);

            }
            return OperationResult.NotFound("user not exisit");
        }

        /// <inheritdoc/>
        public async Task<OperationResult> UpdateSubmitedTime(UpdateSubmitedTimetModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserID);
            if (user != null)
            {
                var repository = _unitOfWork.GetRepository<Timesheet>();
                var item = await repository.GetByIdAsync(model.Id);
                if (item != null)
                {
                    _mapper.Map(model, item);  
                    await repository.UpdateAsync(item);
                    await _unitOfWork.SaveChangesAsync();
                    return OperationResult.Succeeded("Data updated");
                }
                return OperationResult.NotFound();
            }
            return OperationResult.NotFound("user not exisit");

        }

        /// <inheritdoc/>
        public async Task<OperationResult> DeleteSubmitedTime(int id)
        {
            var repository = _unitOfWork.GetRepository<Timesheet>();
            var timesheet = await repository.GetByIdAsync(id);

            if (timesheet == null) return OperationResult.NotFound();
            await repository.DeleteAsync(timesheet.Id);
            await _unitOfWork.SaveChangesAsync();
            return OperationResult.Succeeded();
        }

    }
}
