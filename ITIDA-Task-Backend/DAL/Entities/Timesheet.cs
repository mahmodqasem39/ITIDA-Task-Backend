using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITIDATask.DAL.Entities
{
    public class Timesheet
    {
        public int Id { get; set; }
        public DateTime RegisterDate { get; set; }
        public TimeSpan LoginTime { get; set; }
        public TimeSpan LogoutTime { get; set; }
        public double TotalLoggedHours { get; private set; }
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

    }
}
