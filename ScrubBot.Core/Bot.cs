using Discord;
using Discord.Commands;
using Discord.WebSocket;

using ScrubBot.Managers;
using ScrubBot.Tools;

using System;
using System.Reflection;
using System.Threading.Tasks;

namespace ScrubBot.Core
{
    public class Bot
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commandService;
        private readonly PrefixManager _prefixHandler;

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

            _commandService = new CommandService(new CommandServiceConfig()
                {
                    DefaultRunMode = RunMode.Async,
                    CaseSensitiveCommands = false,
                    LogLevel = LogSeverity.Debug
                }
            );

            _commandService.AddModulesAsync(Assembly.GetAssembly(typeof(Modules.Module))).Wait();
            _commandService.Log += OnCommandServiceLog;

            RegisterServices();
            HookEvents();

            Container.Add(_commandService);
            Container.Add(_client);

            _prefixHandler = Container.Get<PrefixManager>();
        }

        private Task OnCommandServiceLog(LogMessage message)
        {
            Console.WriteLine(message);

            return Task.CompletedTask;
        }

        public async Task InitAsync(string token)
        {
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            await _client.SetGameAsync("Type >Help for help");

            await Task.Delay(-1);
        }

        private void RegisterServices()
        {
            Container.Add<PrefixManager>();
            Container.Add<ChannelManager>();
            Container.Add<GuildManager>();
            Container.Add<RoleManager>();
            Container.Add<UserManager>();
        }

        private void HookEvents()
        {
            _client.Log += OnClientLog;
            _client.Ready += OnClientReadyAsync;
            _client.MessageReceived += OnMessageRecievedAsync;

            ChannelManager channelManager = Container.Get<ChannelManager>();
            _client.ChannelCreated += channelManager.OnChannelCreatedAsync;
            _client.ChannelDestroyed += channelManager.OnChannelDestroyedAsync;
            _client.ChannelUpdated += channelManager.OnChannelUpdatedAsync;

            GuildManager guildManager = Container.Get<GuildManager>();
            _client.GuildAvailable += guildManager.OnGuildAvailableAsync;
            _client.GuildMembersDownloaded += guildManager.OnGuildMembersDownloadedAsync;
            _client.GuildMemberUpdated += guildManager.OnGuildMemberUpdatedAsync;
            _client.GuildUnavailable += guildManager.OnGuildUnavailableAsync;
            _client.GuildUpdated += guildManager.OnGuildUpdatedAsync;
            _client.JoinedGuild += guildManager.OnJoinedGuildAsync;
            _client.LeftGuild += guildManager.OnLeftGuildAsync;

            RoleManager roleManager = Container.Get<RoleManager>();
            _client.RoleCreated += roleManager.OnRoleCreatedAsync;
            _client.RoleDeleted += roleManager.OnRoleDeletedAsync;
            _client.RoleUpdated += roleManager.OnRoleUpdatedAsync;
        }

        private Task OnClientLog(LogMessage message)
        {
            if (!message.Message.Contains("OpCode"))
            {
                Console.WriteLine(message);
            }

            return Task.CompletedTask;
        }

        private async Task OnClientReadyAsync()
        {
            UserManager userManager = Container.Get<UserManager>();
            await userManager.AddUsersAsync(_client.Guilds).ConfigureAwait(false);
        }

        public async Task OnMessageRecievedAsync(SocketMessage msg)
        {
            SocketUserMessage message = msg as SocketUserMessage;

            if (message is null || message.Author.IsBot)
                return;

            string prefix = _prefixHandler.Get((message.Channel as SocketGuildChannel).Guild.Id);
            int argPos = 0;

            bool hasPrefix = message.HasStringPrefix(prefix, ref argPos);
            bool isMentioned = message.HasMentionPrefix(_client.CurrentUser, ref argPos);

            if (!hasPrefix && !isMentioned)
                return;

            SocketCommandContext context = new SocketCommandContext(_client, message);
            IResult result = await _commandService.ExecuteAsync(context, argPos, Container.GetServiceProvider());

            if (!result.IsSuccess)
            {
                Console.WriteLine(new LogMessage(LogSeverity.Error, "Command", result.ErrorReason));
            }
        }
    }
}
