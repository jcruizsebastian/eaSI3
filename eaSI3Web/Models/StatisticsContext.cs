using Microsoft.EntityFrameworkCore;

namespace eaSI3Web.Models
{
    public class StatisticsContext : DbContext
    {
        public StatisticsContext()
        { }

        public StatisticsContext(DbContextOptions<StatisticsContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(user => new { user.JiraUserName, user.SI3UserName }).IsUnique();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<IssueCreation> IssuesCreation { get; set; }
        public DbSet<WorkTracking> WorkTracking { get; set; }
    }
}