using System.Reflection;
using System.Threading.Tasks;

using Discord.Commands;
using Discord.WebSocket;

using ScrubBot.Extensions;
using ScrubBot.Managers;
using ScrubBot.Tools;

using TheKrystalShip.DependencyInjection;

namespace ScrubBot.Core.Commands
{
    public class CommandOperator : CommandService
    {
        private readonly DiscordSocketClient _client;
        private readonly IPrefixManager _prefixManager;

        public CommandOperator(DiscordSocketClient client, CommandServiceConfig config) : base(config)
        {
            _client = client;
            _prefixManager = Container.Get<IPrefixManager>();
        }

        public async Task LoadModulesAsync()
        {
            await AddModulesAsync(Assembly.GetAssembly(typeof(Modules.Module)), Container.GetServiceProvider());

            Store.Set(Commands);
            Store.Set(base.Modules);
        }

        public async Task OnClientMessageReceivedAsync(SocketMessage socketMessage)
        {
            SocketUserMessage message = socketMessage as SocketUserMessage;

            if (message is null)
                return;
            
            if (message.IsValid(_prefixManager.Get((message.Channel as SocketGuildChannel).Guild.Id), _client.CurrentUser, out int argPos))
            {
                SocketCommandContext context = new SocketCommandContext(_client, message);
                _ = await ExecuteAsync(context, argPos, Container.GetServiceProvider());
            }
        }
    }
}
