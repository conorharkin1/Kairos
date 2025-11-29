using DashboardServer.Models;
using DashboardServer.Services;
using DashboardServer.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DashboardServer.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IUserResolverService _userResolverService;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IUserResolverService userResolverService) : base(options)
        {
            _userResolverService = userResolverService;
        }

        public string? CurrentUserId => _userResolverService.UserId;

        public DbSet<Course> Courses { get; set; }
        public DbSet<Assignment> Assignments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Course>().HasQueryFilter(c => c.UserId == CurrentUserId)
                .HasMany(c => c.Assignments)
                .WithOne(a => a.Course)
                .HasForeignKey(a => a.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Assignment>().HasQueryFilter(a => a.UserId == CurrentUserId);
        }
    }
}