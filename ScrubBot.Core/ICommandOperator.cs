using System.Collections.Generic;
using System.Threading.Tasks;

using Discord.Commands;
using Discord.WebSocket;

namespace ScrubBot.Core
{
    public interface ICommandOperator
    {
        IEnumerable<CommandInfo> Commands { get; }
        IEnumerable<ModuleInfo> Modules { get; }
        Task OnClientMessageReceivedAsync(SocketMessage socketMessage);
    }
}
