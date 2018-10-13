using Microsoft.EntityFrameworkCore;

using ScrubBot.Database.Models;

namespace ScrubBot.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }

        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }

        public void MigrateDatabase() => Database.Migrate();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Guild>()
                .HasMany(x => x.Users)
                .WithOne(x => x.Guild)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Guild>()
                .Property(x => x.Id)
                .HasConversion<string>();

            modelBuilder.Entity<Guild>()
                .Property(x => x.AuditChannelId)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .Property(x => x.Id)
                .HasConversion<string>();

            modelBuilder.Entity<Guild>()
                .HasMany(x => x.Events)
                .WithOne(x => x.Guild);

            modelBuilder.Entity<User>()
                .HasMany(x => x.AuthoringEvents)
                .WithOne(x => x.Author);

            modelBuilder.Entity<User>()
                .HasMany(x => x.SubscribedEvents);

            modelBuilder.Entity<Event>()
                .HasMany(x => x.Subscribers);

            modelBuilder.Entity<Event>()
                .HasOne(x => x.Author)
                .WithMany(x => x.AuthoringEvents);
        }
    }
}
