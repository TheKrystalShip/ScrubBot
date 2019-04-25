using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using ScrubBot.Tools;
using TheKrystalShip.Tools.Configuration;

namespace ScrubBot.Modules
{
    [Summary("Test"), Remarks("MoreTest")]
    public class UserModule : Module
    {
        public UserModule()
        {

        }

        [Command("Info"), Summary("Display info about the bot.")]
        public async Task Info()
        {
            Embed embed = EmbedFactory.Create(builder =>
            {
                builder.WithColor(Color.Purple);
                builder.WithTitle("Bot info");
                builder.ThumbnailUrl = Guild.IconUrl;
                builder.AddField("Server Name", Guild.Name ?? "null");
                if (Guild.AuditChannelId != null)
                {
                    SocketTextChannel auditChannel = (SocketTextChannel)Context.Guild.GetChannel(Guild.AuditChannelId.Value);
                    builder.AddField("Audit Channel", auditChannel != null ? auditChannel.Mention : "Invalid channel!");
                }
                builder.AddField("String prefix", Guild.Prefix ?? Configuration.Get("Bot:Prefix"));
            });

            await ReplyAsync(embed);
        }

        [Command("Help")]
        public async Task Help()
        {
            IEnumerable<ModuleInfo> modules = CommandService.Modules;

            Embed embed = EmbedFactory.Create(builder =>
            {
                builder.WithColor(Color.Purple);
                builder.WithTitle("Help ");
                builder.WithDescription("Commands are separated per module. To get all the commands in a module, use Help(moduleName)");

                foreach (var module in modules)
                {
                    if (module.Name == nameof(Module))
                        continue;

                    builder.AddField(module.Name.Replace("Module", string.Empty), !string.IsNullOrEmpty(module.Summary) ? module.Summary : "No summary available");
                }
            });

            await ReplyAsync(embed);
        }

        [Command("Help")]
        public async Task Help(string module)
        {
            CommandInfo[] commands = CommandService.Commands.Where(x => string.Equals(x.Module.Name.Replace("Module", string.Empty), module, StringComparison.CurrentCultureIgnoreCase)).ToArray();

            Embed embed = EmbedFactory.Create(builder =>
            {
                if (commands.Length is 0)
                {
                    builder.WithColor(Color.Red);
                    builder.WithTitle("Error");
                    builder.WithDescription($"No module compares to {module}... \nSee Help for all available modules");
                }
                else
                {
                    builder.WithColor(Color.Purple);
                    builder.WithTitle($"Command list ({module.Replace(module[0], char.ToUpper(module[0]))})");

                    foreach (CommandInfo command in commands)
                    {
                        if (command.Name == "Help")
                            continue;

                        string embedFieldText = $"{(!string.IsNullOrEmpty(command.Summary) ? command.Summary : "No summary available")}\n";

                        string parameters = string.Empty;
                        if (command.Parameters.Count > 0)
                        {
                            parameters += command.Parameters.Aggregate(parameters, (current, param) => current + $"{param.Name} ");
                            parameters = parameters.Insert(0, "( ");
                            parameters += ")";
                        }
                        builder.AddField($"\n{command.Name} \t {parameters}", $"{embedFieldText}\n ");
                    }
                }
            });

            await ReplyAsync(embed);
        }

        [Command("Reply")]
        public async Task<RuntimeResult> Reply(SocketGuildUser userToReplyTo, int prevMessageIndex, [Remainder]string reply)
        {
            if (prevMessageIndex < 1)
                return new ErrorResult($"The number telling me which message you want to reply to, must be at least 1!");

            const int messageLogLength = 20;
            IEnumerable<IMessage> lastMessages = await Context.Channel.GetMessagesAsync(messageLogLength).FlattenAsync();
            IEnumerable<IMessage> enumerable = lastMessages as IMessage[] ?? lastMessages.ToArray();

            if (enumerable.Count() < prevMessageIndex)
                return new ErrorResult(EmbedFactory.Create(x =>
                {
                    x.WithColor(Color.Red);
                    x.WithTitle($"There are less than {prevMessageIndex} messages in this channel, let alone from {userToReplyTo.Username}... Please reconsider the command!");
                }));

            if (enumerable.First(x => x.Author == userToReplyTo) is null)
                return new ErrorResult(EmbedFactory.Create(x =>
                {
                    x.WithColor(Color.Red);
                    x.WithTitle($"{userToReplyTo.Username} hasn't sent a message in the last {messageLogLength} messages.");
                }));

            if (enumerable.Count(x => x.Author == userToReplyTo) < prevMessageIndex)
                return new ErrorResult(EmbedFactory.Create(x =>
                {
                    x.WithColor(Color.Red);
                    x.WithTitle($"{userToReplyTo.Username} hasn't sent {prevMessageIndex} messages in the last {messageLogLength} messages.");
                }));

            var messageToReplyTo = enumerable.Where(x => x.Author == userToReplyTo).ToArray()[prevMessageIndex - (userToReplyTo == Context.User ? 0 : 1)];

            if (messageToReplyTo.Attachments.Count > 0 || messageToReplyTo.Embeds.Count > 0)
                return new ErrorResult(EmbedFactory.Create(x =>
                {
                    x.WithColor(Color.Red);
                    x.WithTitle("Error");
                    x.WithDescription("Sadly, I'm unable to handle replying to messages containing attachments or embeds... sorry 😕");
                }));

            return new InfoResult(EmbedFactory.Create(x =>
            {
                x.WithColor(Color.Purple);
                x.WithDescription(userToReplyTo.Mention);
                x.AddField("Original:", messageToReplyTo.Content);
                x.AddField($"Reply:", reply);
            }));
        }

        [Command("Reply")]
        public async Task<RuntimeResult> Reply(ulong messageId, [Remainder] string reply)
        {
            if (!(Context.Channel is ITextChannel currentChannel))
                return new ErrorResult(EmbedFactory.Create(x =>
                {
                    x.WithColor(Color.Red);
                    x.WithTitle("Error");
                    x.WithDescription("Sorry, was unable to cast the current channel to ITextChannel");
                }));

            IMessage message = await currentChannel.GetMessageAsync(messageId);

            if (message is null)
                return new ErrorResult(EmbedFactory.Create(x =>
                {
                    x.WithColor(Color.Red);
                    x.WithTitle("Error");
                    x.WithDescription("Sorry, was unable to find the requested message");
                }));

            if (message.Attachments.Count > 0 || message.Embeds.Count > 0)
                return new ErrorResult(EmbedFactory.Create(x =>
                {
                    x.WithColor(Color.Red);
                    x.WithColor(Color.Red);
                    x.WithDescription("Sadly, I'm unable to handle replying to messages containing attachments or embeds... sorry 😕");
                }));

            return new InfoResult(EmbedFactory.Create(x =>
            {
                x.WithColor(Color.Purple);
                x.WithDescription(message.Author.Mention);
                x.AddField("Original:", message.Content);
                x.AddField($"Reply:", reply);
            }));
        }
    }
}
