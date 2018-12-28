using TheKrystalShip.DependencyInjection;

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
            Users = Container.Get<IUserManager>();
            Guilds = Container.Get<IGuildManager>();
            Roles = Container.Get<IRoleManager>();
            Channels = Container.Get<IChannelManager>();
            Prefixes = Container.Get<IPrefixManager>();
        }
    }
}
