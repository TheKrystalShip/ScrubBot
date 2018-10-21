using Discord;
using Discord.WebSocket;

using ScrubBot.Handlers;
using ScrubBot.Managers;
using ScrubBot.Properties;

using System;
using System.Threading.Tasks;

namespace ScrubBot
{
    public class Bot
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandHandler _commandHandler;

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

            Container.Add(_client);

            _commandHandler = new CommandHandler(ref _client);
        }

        public async Task InitAsync()
        {
            await _client.LoginAsync(TokenType.Bot, Configuration.Get("Bot:Token"));
            await _client.StartAsync();
            await _client.SetGameAsync("Type >Help for help");
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

            return Task.CompletedTask;
        }
    }
}
