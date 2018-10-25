using System.Threading.Tasks;
using Discord.WebSocket;

namespace ScrubBot.Core
{
    internal interface ICommandOperator
    {
        Task ExecuteAsync(SocketMessage socketMessage);
    }
}