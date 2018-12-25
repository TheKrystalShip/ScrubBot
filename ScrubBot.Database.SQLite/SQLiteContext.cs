using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using ScrubBot.Database.Domain;
using ScrubBot.Database.SQLite.Configurations;

namespace ScrubBot.Database.SQLite
{
    public class SQLiteContext : DbContext, IDbContext
    {
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }

        public SQLiteContext(DbContextOptions options) : base(options)
        {

        }

        public void Migrate()
        {
            Database.Migrate();
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new EventConfiguration());
            modelBuilder.ApplyConfiguration(new GuildConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }

        void IDbContext.SaveChanges()
        {
            SaveChanges();
        }

        public Task SaveChangesAsync()
        {
            return SaveChangesAsync();
        }
    }
}
