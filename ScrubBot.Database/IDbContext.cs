using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using ScrubBot.Database.Domain;

namespace ScrubBot.Database
{
    public interface IDbContext
    {
        DbSet<Event> Events { get; set; }
        DbSet<Guild> Guilds { get; set; }
        DbSet<User> Users { get; set; }

        void Migrate();
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
