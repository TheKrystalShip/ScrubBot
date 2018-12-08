using System.Reflection;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using ScrubBot.Core.Commands;
using ScrubBot.Extensions;
using ScrubBot.Managers;
using ScrubBot.Tools;

namespace ScrubBot.Core
{
    internal class CommandOperator
    {
        private readonly Bot _client;
        private readonly CommandService _commandService;
        private readonly IPrefixManager _prefixManager;

        public CommandOperator(Bot client)
        {
            _client = client;
            _prefixManager = Container.Get<PrefixManager>();

            _commandService = new CommandService(new CommandServiceConfig()
                {
                    DefaultRunMode = RunMode.Async,
                    CaseSensitiveCommands = false,
                    LogLevel = LogSeverity.Debug
                }
            );

            _commandService.AddModulesAsync(Assembly.GetAssembly(typeof(Modules.Module))).Wait();
            _commandService.Log += Logger.Log;
            _commandService.CommandExecuted += Dispatcher.Dispatch;

            Container.Add(_commandService);
        }

        public async Task ExecuteAsync(SocketMessage socketMessage)
        {
            SocketUserMessage message = socketMessage as SocketUserMessage;

            if (message is null)
            {
                return;
            }

            string prefix = _prefixManager.Get((message.Channel as SocketGuildChannel).Guild.Id);

            if (message.IsValid(prefix, _client.CurrentUser, out int argPos))
            {
                SocketCommandContext context = new SocketCommandContext(_client, message);
                await _commandService.ExecuteAsync(context, argPos, Container.GetServiceProvider());
            }
        }
    }
}
