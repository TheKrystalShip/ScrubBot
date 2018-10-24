using Discord;
using Discord.Commands;
using Discord.WebSocket;

using ScrubBot.Handlers;
using ScrubBot.Managers;

using System;
using System.Reflection;
using System.Threading.Tasks;

namespace ScrubBot
{
    public class Bot
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commandService;
        private readonly PrefixHandler _prefixHandler;

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

            _client.Log += OnClientLog;
            _client.Ready += OnClientReady;
            _client.MessageReceived += OnMessageRecievedAsync;

            _commandService = new CommandService(new CommandServiceConfig()
                {
                    DefaultRunMode = RunMode.Async,
                    CaseSensitiveCommands = false,
                    LogLevel = LogSeverity.Debug
                }
            );

            _commandService.AddModulesAsync(Assembly.GetEntryAssembly()).Wait();
            _commandService.Log += OnClientLog;

            HookEvents();

            Container.Add(_client);
            Container.Add(_commandService);
            _prefixHandler = Container.Get<PrefixHandler>();
        }

        public async Task InitAsync()
        {
            await _client.LoginAsync(TokenType.Bot, Configuration.Get("Bot:Token"));
            await _client.StartAsync();
            await _client.SetGameAsync("Type >Help for help");

            await Task.Delay(-1);
        }

        private void HookEvents()
        {
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

        private Task OnClientReady()
        {
            try
            {
                UserManager userManager = Container.Get<UserManager>();

                Task.Run(async () =>
                {
                    foreach (SocketGuild guild in _client.Guilds)
                    {
                        foreach (SocketGuildUser user in guild.Users)
                        {
                            if (user.IsBot)
                                continue;

                            await userManager.AddUserAsync(user).ConfigureAwait(false);
                        }
                    }
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                Container.Init();
            }

            return Task.CompletedTask;
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
