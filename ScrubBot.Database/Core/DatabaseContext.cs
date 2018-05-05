using ScrubBot.Database.Models;
using ScrubBot.Database.Properties;

using Microsoft.EntityFrameworkCore;

namespace ScrubBot.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<User> Users { get; set; }

        public DatabaseContext() : base() => Database.Migrate();

        public void MigrateDatabase() => Database.Migrate();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(Resources.ConnectionString);
            optionsBuilder.EnableSensitiveDataLogging(true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Guild>()
                .HasMany(x => x.Users)
                .WithOne(x => x.Guild)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}