using Discord;
using Discord.WebSocket;

using ScrubBot.Managers;
using ScrubBot.Tools;
using System.Threading.Tasks;

namespace ScrubBot.Core
{
    public class Bot : IBot
    {
        private readonly DiscordSocketClient _client;
        private readonly IManager _manager;
        private CommandOperator _commandOperator;

        public Bot()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug,
                DefaultRetryMode = RetryMode.AlwaysRetry,
                ConnectionTimeout = 5000,
                AlwaysDownloadUsers = true
            }
            );

            _manager = Container.Get<Manager>();

            RegisterServices();
            HookEvents();

            Container.Add(_client);
        }

        public async Task InitAsync(string token)
        {
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            await _client.SetGameAsync("Type >Help for help");

            //await Task.Delay(-1);
        }

        private void RegisterServices()
        {
            Container.Add<IPrefixManager, PrefixManager>();
            Container.Add<IChannelManager, ChannelManager>();
            Container.Add<IGuildManager, GuildManager>();
            Container.Add<IRoleManager, RoleManager>();
            Container.Add<IUserManager, UserManager>();
            Container.Add<ICommandOperator, CommandOperator>();
        }

        private void HookEvents()
        {
            _client.Log += Logger.Log;
            _client.Ready += OnClientReadyAsync;
            _client.MessageReceived += OnMessageReceivedAsync;

            _client.ChannelCreated += _manager.Channels.OnChannelCreatedAsync;
            _client.ChannelDestroyed += _manager.Channels.OnChannelDestroyedAsync;
            _client.ChannelUpdated += _manager.Channels.OnChannelUpdatedAsync;

            _client.GuildAvailable += _manager.Guilds.OnGuildAvailableAsync;
            _client.GuildMembersDownloaded += _manager.Guilds.OnGuildMembersDownloadedAsync;
            _client.GuildMemberUpdated += _manager.Guilds.OnGuildMemberUpdatedAsync;
            _client.GuildUnavailable += _manager.Guilds.OnGuildUnavailableAsync;
            _client.GuildUpdated += _manager.Guilds.OnGuildUpdatedAsync;
            _client.JoinedGuild += _manager.Guilds.OnJoinedGuildAsync;
            _client.LeftGuild += _manager.Guilds.OnLeftGuildAsync;

            _client.RoleCreated += _manager.Roles.OnRoleCreatedAsync;
            _client.RoleDeleted += _manager.Roles.OnRoleDeletedAsync;
            _client.RoleUpdated += _manager.Roles.OnRoleUpdatedAsync;
        }

        private async Task OnClientReadyAsync()
        {
            _commandOperator = Container.Get<CommandOperator>();
            await _manager.Guilds.AddGuildsAsync(_client.Guilds).ConfigureAwait(false);
            await _manager.Users.AddUsersAsync(_client.Guilds).ConfigureAwait(false);
        }

        private async Task OnMessageReceivedAsync(SocketMessage message) => await (_commandOperator ?? (_commandOperator = Container.Get<CommandOperator>())).ExecuteAsync(message).ConfigureAwait(false);
    }
}
