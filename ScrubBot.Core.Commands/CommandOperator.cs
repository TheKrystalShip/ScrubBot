using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

using Discord.Commands;
using Discord.WebSocket;

using ScrubBot.Extensions;
using ScrubBot.Managers;

using TheKrystalShip.DependencyInjection;

namespace ScrubBot.Core.Commands
{
    public class CommandOperator : CommandService, ICommandOperator
    {
        private readonly DiscordSocketClient _client;
        private readonly IPrefixManager _prefixManager;
        public new IEnumerable<CommandInfo> Commands => base.Commands;
        public new IEnumerable<ModuleInfo> Modules => base.Modules;

        public CommandOperator(DiscordSocketClient client, CommandServiceConfig config) : base(config)
        {
            _client = client;
            _prefixManager = Container.Get<IPrefixManager>();
        }

        public async Task LoadModulesAsync()
        {
            await AddModulesAsync(Assembly.GetAssembly(typeof(Modules.Module)), Container.GetServiceProvider());
        }

        public async Task OnClientMessageReceivedAsync(SocketMessage socketMessage)
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
                IResult result = await ExecuteAsync(context, argPos, Container.GetServiceProvider());
            }
        }
    }
}
