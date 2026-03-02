using DashboardServer.Models;
using DashboardServer.Models.Constraints;
using DashboardServer.Services.Interfaces;
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
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<DailyCapacity> DailyCapacities { get; set; }
        public DbSet<CourseWeight> CourseWeights { get; set; }
        public DbSet<SchedulerRules> SchedulerRules { get; set; }
        public DbSet<ScheduleBlock> ScheduleBlocks { get; set; }
        public DbSet<AvoidPeriod> AvoidPeriods { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Global Query Filters
            builder.Entity<Course>().HasQueryFilter(c => c.UserId == CurrentUserId);
            builder.Entity<Assignment>().HasQueryFilter(a => a.UserId == CurrentUserId);
            builder.Entity<Quiz>().HasQueryFilter(q => q.UserId == CurrentUserId);
            builder.Entity<DailyCapacity>().HasQueryFilter(d => d.UserId == CurrentUserId);
            builder.Entity<CourseWeight>().HasQueryFilter(cw => cw.UserId == CurrentUserId);
            builder.Entity<SchedulerRules>().HasQueryFilter(sr => sr.UserId == CurrentUserId);
            builder.Entity<ScheduleBlock>().HasQueryFilter(sb => sb.UserId == CurrentUserId);
            builder.Entity<Schedule>().HasQueryFilter(s => s.UserId == CurrentUserId);
            builder.Entity<AvoidPeriod>().HasQueryFilter(ap => ap.UserId == CurrentUserId);

            // Relationships
            builder.Entity<Course>()
                .HasMany(c => c.Assignments)
                    .WithOne(a => a.Course)
                    .HasForeignKey(a => a.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Course>()
                .HasMany(c => c.Quizzes)
                    .WithOne(q => q.Course)
                    .HasForeignKey(q => q.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Schedule>()
                .HasMany(s => s.Blocks)
                    .WithOne(b => b.Schedule)
                    .HasForeignKey(b => b.ScheduleId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Schedule>()
                .HasOne(s => s.User)
                .WithOne(u => u.Schedule)
                .HasForeignKey<Schedule>(s => s.UserId);

            
            builder.Entity<DailyCapacity>()
                .HasIndex(dc => new { dc.UserId, dc.DayIndex })
                .IsUnique();

            builder.Entity<CourseWeight>()
                .HasIndex(cw => new { cw.UserId, cw.CourseId })
                .IsUnique();
            
            builder.Entity<AvoidPeriod>()
                .HasMany(ap => ap.AvoidTimeBlocks)
                    .WithOne()
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}