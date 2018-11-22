using Discord;
using Discord.WebSocket;

using ScrubBot.Managers;
using ScrubBot.Tools;
using System.Threading.Tasks;

namespace ScrubBot.Core
{
    public class Bot : DiscordSocketClient
    {
        private readonly IManager _manager;
        private CommandOperator _commandOperator;

        public Bot() : this(new DiscordSocketConfig() { LogLevel = LogSeverity.Debug, DefaultRetryMode = RetryMode.AlwaysRetry })
        {

        }

        public Bot(DiscordSocketConfig config) : base(config)
        {
            _manager = Container.Get<Manager>();
            HookEvents();

            Container.Add<CommandOperator>();
            Container.Add(this);
        }

        public async Task InitAsync(string token)
        {
            await LoginAsync(TokenType.Bot, token);
            await StartAsync();
            await SetGameAsync("Type >Help for help");
        }

        private void HookEvents()
        {
            Log += Logger.Log;
            Ready += OnClientReadyAsync;
            MessageReceived += OnMessageReceivedAsync;

            ChannelCreated += _manager.Channels.OnChannelCreatedAsync;
            ChannelDestroyed += _manager.Channels.OnChannelDestroyedAsync;
            ChannelUpdated += _manager.Channels.OnChannelUpdatedAsync;

            GuildAvailable += _manager.Guilds.OnGuildAvailableAsync;
            GuildMembersDownloaded += _manager.Guilds.OnGuildMembersDownloadedAsync;
            GuildMemberUpdated += _manager.Guilds.OnGuildMemberUpdatedAsync;
            GuildUnavailable += _manager.Guilds.OnGuildUnavailableAsync;
            GuildUpdated += _manager.Guilds.OnGuildUpdatedAsync;
            JoinedGuild += _manager.Guilds.OnJoinedGuildAsync;
            LeftGuild += _manager.Guilds.OnLeftGuildAsync;

            RoleCreated += _manager.Roles.OnRoleCreatedAsync;
            RoleDeleted += _manager.Roles.OnRoleDeletedAsync;
            RoleUpdated += _manager.Roles.OnRoleUpdatedAsync;
        }

        private async Task OnClientReadyAsync()
        {
            _commandOperator = Container.Get<CommandOperator>();
            await _manager.Guilds.AddGuildsAsync(Guilds).ConfigureAwait(false);
            await _manager.Users.AddUsersAsync(Guilds).ConfigureAwait(false);
        }

        private async Task OnMessageReceivedAsync(SocketMessage message) => await _commandOperator.ExecuteAsync(message).ConfigureAwait(false);
    }
}
