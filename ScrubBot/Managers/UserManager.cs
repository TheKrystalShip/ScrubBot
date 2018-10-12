using ScrubBot.Database;
using ScrubBot.Database.Models;

namespace ScrubBot.Managers
{
    public class UserManager : IManager<User>
    {
        private readonly DatabaseContext _context;

        public UserManager(DatabaseContext context)
        {
            _context = context;
        }

        public User Get(ulong id)
        {
            return _context.Users.Find(id);
        }
    }
}
