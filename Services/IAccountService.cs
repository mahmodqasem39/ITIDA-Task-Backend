using ITIDATask.Utitlites;

namespace ITIDATask.Services
{
    public interface IAccountService
    {
        Task<OperationResult> RegisterAsync(RegisterModel registerModel);
        Task<OperationResult> ValidateUserAsync(LoginModel registerModel);

    }
}
