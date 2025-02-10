using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITIDATask.DAL.Entities
{
    public class Timesheet
    {
        public int Id { get; set; }
        public DateOnly RegisterDate { get; set; }
        public TimeOnly LoginTime { get; set; }
        public TimeOnly LogoutTime { get; set; }
        public double TotalLoggedHours { get; private set; }
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

    }
}
