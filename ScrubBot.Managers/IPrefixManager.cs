using System.Threading.Tasks;

namespace ScrubBot.Managers
{
    public interface IPrefixManager
    {
        string Get(ulong guildId);
        Task<bool> SetAsync(ulong guildId, string prefix);
    }
}