using ITIDATask.DAL.Entities;
using ITIDATask.Utitlites;

namespace ITIDATask.Services
{
    public interface ITimesheetService
    {
        /// <summary>
        /// Submits a registered timesheet entry for a user.
        /// </summary>
        /// <param name="submitModel">The timesheet model containing the user's registered time details.</param>
        /// <returns>
        /// An <see cref="OperationResult"/> indicating the result of the operation:
        /// - Returns <see cref="OperationResult.Failed(string)"/> if the date is in the future or if logout time is before login time.
        /// - Returns <see cref="OperationResult.Existed(string)"/> if a timesheet entry already exists for the user on the given date.
        /// - Returns <see cref="OperationResult.Succeeded()"/> if the timesheet entry is successfully added.
        /// </returns>
        Task<OperationResult> SubmitRegisterdTime(Timesheet submitModel);
        /// <summary>
        /// Getall submiteed timesheet entries for a specific user, ordered by the registration date in descending order.
        /// </summary>
        /// <param name="userId"> userId whose timesheet records are to be fetched.</param>
        /// <returns>
        /// An <see cref="OperationResult"/> containing the list of timesheet records mapped to <see cref="TimeSheetModel"/> 
        /// if the user exists; otherwise, returns an error indicating that the user was not found.
        /// </returns>
        Task<OperationResult> GetAllRegiterdTime(string userId);
        /// <summary>
        /// Updates a submitted timesheet entry for a specific user.
        /// </summary>
        /// <param name="model">The <see cref="UpdateSubmitedTimetModel"/> containing the updated timesheet details.</param>
        /// <returns>
        /// An <see cref="OperationResult"/> indicating success if the timesheet is updated successfully; 
        /// otherwise, returns an error if the user or timesheet entry is not found.
        /// </returns>
        Task<OperationResult> UpdateSubmitedTime(UpdateSubmitedTimetModel model);
        /// <summary>
        /// Deletes a submitted timesheet entry based on the provided item Id.
        /// </summary>
        /// <param name="id">item Id of timesheet entry to be deleted.</param>
        /// <returns>
        /// An <see cref="OperationResult"/> indicating success if the timesheet entry is deleted successfully; 
        /// otherwise, returns an error if the entry is not found.
        /// </returns>
        Task<OperationResult> DeleteSubmitedTime(int id);


    }
}
