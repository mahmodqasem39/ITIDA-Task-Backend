using ITIDATask.Utitlites;

namespace ITIDATask.Services
{
    public interface IAccountService
    {
        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="registerModel">The <see cref="RegisterModel"/> containing user registration details.</param>
        /// <returns>
        /// An <see cref="OperationResult"/> indicating success if the user is registered successfully;  
        /// if the user already exists, returns an error; otherwise, returns a failure result with the user payload.
        /// </returns>
        Task<OperationResult> RegisterAsync(RegisterModel registerModel);
        /// <summary>
        /// Validates a user's login credentials and generates an access token if authentication is successful.
        /// </summary>
        /// <param name="loginModel">The <see cref="LoginModel"/> containing the user's login credentials.</param>
        /// <returns>
        /// An <see cref="OperationResult"/> indicating success if the user is authenticated successfully, 
        /// returning a JWT token and expiration details. 
        /// If authentication fails, it returns an error message with details.
        /// </returns>
        Task<OperationResult> ValidateUserAsync(LoginModel registerModel);
        Task SignOut();
    }
}
