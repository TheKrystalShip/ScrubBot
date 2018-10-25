using ScrubBot.Tools;

namespace ScrubBot.Managers
{
    public class Manager : IManager
    {
        public IUserManager Users { get; private set; }
        public IGuildManager Guilds { get; private set; }
        public IRoleManager Roles { get; private set; }
        public IChannelManager Channels { get; private set; }
        public IPrefixManager Prefixes { get; private set; }

        public Manager()
        {
            Users = Container.Get<UserManager>();
            Guilds = Container.Get<GuildManager>();
            Roles = Container.Get<RoleManager>();
            Channels = Container.Get<ChannelManager>();
            Prefixes = Container.Get<PrefixManager>();
        }
    }
}
