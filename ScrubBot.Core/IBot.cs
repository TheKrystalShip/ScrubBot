using System.Threading.Tasks;

namespace ScrubBot.Core
{
    public interface IBot
    {
        Task InitAsync(string token);
    }
}