using ITIDATask.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace ITIDATask.Utitlites
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Name Is Required")]
        public required string Name { get; set; }
        [Required(ErrorMessage = "Email Is Required")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "Password Is Required")]
        public required string Password { get; set; }
        [Required(ErrorMessage = "Mobile Number Is Required")]
        public required string MobileNumber { get; set; }
    }


    public class LoginModel
    {
        [Required(ErrorMessage = "Email Is Required")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "Password Is Required")]
        public required string Password { get; set; }
    }

    public class UserViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Id { get; set; }  
    }


    public class LoginResponse
    {
        public string token { get; set; }
        public DateTime ExpiresIn { get; set; }
        //public UserViewModel User { get; set; }
    }

    public class TokenModel
    {
        public string AccessToken { get; set; }
        public DateTime Expires { get; set; }
    }


    public class SubmitTimeModel
    {
        public DateTime RegisterDate { get; set; }
        public TimeSpan LoginTime { get; set; }
        public TimeSpan LogoutTime { get; set; }
        public string UserID { get; set; }

    }


    public class TimeSheetModel
    {
        public int Id { get; set; }
        [Required]
        public DateOnly RegisterDate { get ; set; }
        [Required]
        public TimeSpan LoginTime { get; set; }
        [Required]
        public TimeSpan LogoutTime { get; set; }
        [Required]
        public int TotalLoggedHours { get; set; }
        public string UserID { get; set; }

    }

    public class UpdateSubmitedTimetModel
    {
        public int Id { get; set; }
        public DateTime RegisterDate { get; set; }
        public TimeSpan LoginTime { get; set; }
        public TimeSpan LogoutTime { get; set; }
        public string UserID { get; set; }

    }
    public class AppSettings
    {
        public string Secret { get; set; }
    }
}
