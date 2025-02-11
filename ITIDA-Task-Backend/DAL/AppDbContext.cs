using ITIDATask.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using System;
using System.Data;
using System.Reflection;

namespace ITIDATask.DAL
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, AppRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public virtual DbSet<Timesheet> Timesheets { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var timeOnlyConverter = new ValueConverter<TimeOnly, TimeSpan>(
                v => v.ToTimeSpan(),
                v => TimeOnly.FromTimeSpan(v));

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
              modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Timesheets) 
                .WithOne(t => t.ApplicationUser)
                 .HasForeignKey(t => t.UserId);

            modelBuilder.Entity<Timesheet>(entity =>
            {
                entity.Property(e => e.TotalLoggedHours)
                      .HasComputedColumnSql("TIMESTAMPDIFF(SECOND, LoginTime, LogoutTime) / 3600.0", true);
            });
        }

    }
}
