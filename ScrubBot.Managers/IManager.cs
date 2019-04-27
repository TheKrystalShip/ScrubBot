namespace ScrubBot.Managers
{
    public interface IManager
    {
        IChannelManager Channels { get; }
        IGuildManager Guilds { get; }
        IPrefixManager Prefixes { get; }
        IRoleManager Roles { get; }
        IUserManager Users { get; }
    }
}
