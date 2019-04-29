using TheKrystalShip.DependencyInjection;

namespace ScrubBot.Managers
{
    public class Manager : IManager
    {
        public IUserManager Users { get; }
        public IGuildManager Guilds { get; }
        public IRoleManager Roles { get; }
        public IChannelManager Channels { get; }
        public IPrefixManager Prefixes { get; }
        public IReactionManager Reactions { get; }

        public Manager()
        {
            Users = Container.Get<IUserManager>();
            Guilds = Container.Get<IGuildManager>();
            Roles = Container.Get<IRoleManager>();
            Channels = Container.Get<IChannelManager>();
            Prefixes = Container.Get<IPrefixManager>();
            Reactions = Container.Get<IReactionManager>();
        }
    }
}
