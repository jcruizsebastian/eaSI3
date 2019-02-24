﻿using Microsoft.EntityFrameworkCore;

namespace eaSI3Web.Models
{
    public class StatisticsContext : DbContext
    {
        public StatisticsContext()
        { }

        public StatisticsContext(DbContextOptions<StatisticsContext> options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=statistics.db");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<IssueCreation> IssuesCreation { get; set; }
        public DbSet<WorkTracking> WorkTracking { get; set; }
    }
}