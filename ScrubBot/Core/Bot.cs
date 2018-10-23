using Discord;
using Discord.WebSocket;

using ScrubBot.Handlers;
using ScrubBot.Managers;

using System;
using System.Threading.Tasks;

namespace ScrubBot
{
    public class Bot
    {
        private readonly DiscordSocketClient _client;

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
        }

        public async Task InitAsync()
        {
            await _client.LoginAsync(TokenType.Bot, Configuration.Get("Bot:Token"));
            await _client.StartAsync();
            await _client.SetGameAsync("Type >Help for help");

            Container.Add(_client);
        }

        public Bot HookEvents()
        {
            CommandHandler commandHandler = new CommandHandler(_client);
            _client.MessageReceived += commandHandler.OnMessageRecievedAsync;

            _client.Log += OnClientLog;
            _client.Ready += OnClientReady;

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

            return this;
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
            Container.Init();

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

            return Task.CompletedTask;
        }
    }
}
