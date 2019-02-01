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
                builder.AddField("String prefix", Guild.Prefix ?? Configuration.Get("Prefix:Default"));
            });

            await ReplyAsync(embed);
        }

        [Command("Help")]
        public async Task Help()
        {
            List<CommandInfo> commands = CommandService.Commands.ToList();

            Embed embed = EmbedFactory.Create(builder =>
            {
                builder.WithColor(Color.Purple);
                builder.WithTitle("Command list");

                foreach (CommandInfo command in commands)
                {
                    if (command.Name == "Help")
                        continue;

                    string embedFieldText = command.Summary ?? "No description available\n";

                    if (command.Parameters.Count > 0)
                        embedFieldText = command.Parameters.Aggregate(embedFieldText, (current, param) => current + $"\nParameters:\t{param.Type.Name} {param}\t");

                    builder.AddField($"{command.Name} ({command.Module.Name.Replace("Module", "")})", embedFieldText);
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

            if (messageToReplyTo.Attachments.Count > 0)
                return new ErrorResult(EmbedFactory.Create(x =>
                {
                    x.WithColor(Color.Red);
                    x.WithTitle("Sadly, I'm unable to handle replying to messages containing attachments... sorry 😕");
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
