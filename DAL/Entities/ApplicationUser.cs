using Microsoft.AspNetCore.Identity;

namespace ITIDATask.DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

        public ICollection<Timesheet> Timesheets { get; set; }
    }
}
