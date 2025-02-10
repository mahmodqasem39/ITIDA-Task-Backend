﻿using AutoMapper;
using ITIDATask.DAL.Entities;
using ITIDATask.Repositories.Interfaces;
using ITIDATask.Utitlites;

namespace ITIDATask.Services
{
    public class TimesheetService : ITimesheetService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TimesheetService(IUnitOfWork unitOfWork,IMapper mapper)
        {
           _unitOfWork = unitOfWork;
            _mapper = mapper;
        }



        public async Task<OperationResult> SubmitRegisterdTime(Timesheet submitModel)
        {
            var currentDate = DateOnly.FromDateTime(DateTime.Now); 
            if(submitModel.RegisterDate > currentDate)
            {
                return OperationResult.Failed("Date cannot be in the future");
            }

            if (submitModel.LoginTime >= submitModel.LogoutTime)
            {
                return OperationResult.Failed("Logout time must be after login time." );
            }

            var subimtedBefor = await _unitOfWork.GetRepository<Timesheet>()
                        .FindAsync(x => x.RegisterDate == submitModel.RegisterDate && x.UserId == submitModel.UserId);

            var test = subimtedBefor.FirstOrDefault();
            if (test == null)
            {
                var repo = _unitOfWork.GetRepository<Timesheet>();
                await repo.AddAsync(submitModel);
                await _unitOfWork.SaveChangesAsync();
                return OperationResult.Succeeded();
            }
            return OperationResult.Existed("This date Is submited befor for this user");
        }

        public async Task<OperationResult> GetAllRegiterdTime(string userId)
        {
            var repository = _unitOfWork.GetRepository<Timesheet>();
            var timesheets = await repository.FindAsync(x => x.UserId == userId);
            var timesheetsDto =  _mapper.Map<IEnumerable<TimeSheetModel>>(timesheets);
            return OperationResult.Succeeded(payload: timesheetsDto);
        }

        public async Task<OperationResult> UpdateSubmitedTime(UpdateSubmitedTimetModel model)
        {
            var timesheet = _mapper.Map<Timesheet>(model);
            var repository = _unitOfWork.GetRepository<Timesheet>();
            await repository.UpdateAsync(timesheet);
            await _unitOfWork.SaveChangesAsync();
            return OperationResult.Succeeded("Data updated");
        }


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
