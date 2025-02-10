using ITIDATask.DAL.Entities;
using ITIDATask.Utitlites;

namespace ITIDATask.Services
{
    public interface ITimesheetService
    {
        Task<OperationResult> SubmitRegisterdTime(Timesheet submitModel);
        Task<OperationResult> GetAllRegiterdTime(string userId);
        Task<OperationResult> UpdateSubmitedTime(UpdateSubmitedTimetModel model);
        Task<OperationResult> DeleteSubmitedTime(int id);


    }
}
