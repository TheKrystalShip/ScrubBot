using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using ScrubBot.Extensions;
using ScrubBot.Tools;

namespace ScrubBot.Modules
{
    [Summary("Test")]
    public class UserModule : Module
    {
        public UserModule()
        {

        }

        [Command("Info"), Summary("Display info about the server.")]
        public async Task Info()
        {
            Embed embed = EmbedFactory.Create(x =>
            {
                x.WithColor(Color.Purple);
                x.ThumbnailUrl = Guild.IconUrl;
                x.AddField("Server Name", Guild.Name ?? "null");

                if (Guild.AuditChannelId != null)
                {
                    SocketTextChannel auditChannel = (SocketTextChannel)Context.Guild.GetChannel(Guild.AuditChannelId.Value);
                    x.AddField("Audit Channel", auditChannel != null ? auditChannel.Mention : "Invalid channel!");
                }

                x.AddField("Command prefix", Prefix);
            });

            await ReplyAsync(embed);
        }

        [Command("Help")]
        public async Task<RuntimeResult> Help()
        {
            IEnumerable<ModuleInfo> modules = Store.Get<IEnumerable<ModuleInfo>>();

            Embed embed = EmbedFactory.Create(builder =>
            {
                builder.WithColor(Color.Purple);
                builder.WithTitle("Help ");
                builder.WithDescription($"Commands are separated per module. To get all the commands in a module, use {Prefix}Help moduleName");

                foreach (var module in modules)
                {
                    if (module.Name == nameof(Module))
                        continue;

                    string remarks = string.IsNullOrEmpty(module.Remarks) ? string.Empty : $"({module.Remarks})";

                    builder.AddField(module.Name.Replace("Module", string.Empty), $"{(!string.IsNullOrEmpty(module.Summary) ? module.Summary : "No summary available")}\n{remarks}");
                }
            });

            return new SuccessResult(embed);
        }

        [Command("Help")]
        public async Task<RuntimeResult> Help(string module)
        {
            CommandInfo[] commands = Store.Get<IEnumerable<CommandInfo>>()
                .Where(x => string.Equals(x.Module.Name.Replace("Module", string.Empty), module, StringComparison.CurrentCultureIgnoreCase))
                .ToArray();

            Embed embed = EmbedFactory.Create(builder =>
            {
                if (commands.Length is 0)
                {
                    builder.CreateError($"No module compares to {module}... \nSee Help for all available modules");
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

            return new SuccessResult(embed);
        }

        [Command("Reply"), Summary("Send a message, with a specific message you want to reply to embedded, by counting the amount of messages of a specific user from new to old. DOES NOT WORK WITH ATTACHMENTS OR OTHER EMBEDS!")]
        public async Task<RuntimeResult> Reply(SocketGuildUser userToReplyTo, int prevMessageIndex, [Remainder]string reply)
        {
            if (prevMessageIndex < 1)
                return new ErrorResult("The number telling me which message you want to reply to, must be at least 1!");

            const int messageLogLength = 20;
            IEnumerable<IMessage> lastMessages = await Context.Channel.GetMessagesAsync(messageLogLength).FlattenAsync();
            IEnumerable<IMessage> enumerable = lastMessages as IMessage[] ?? lastMessages.ToArray();

            if (enumerable.Count() < prevMessageIndex)
                return new ErrorResult(EmbedFactory.Create(x =>
                {
                    x.CreateError($"There are less than {prevMessageIndex} messages in this channel, let alone from {userToReplyTo.Username}... Please reconsider the command!");
                }));

            if (enumerable.First(x => x.Author == userToReplyTo) is null)
                return new ErrorResult(EmbedFactory.Create(x =>
                {
                    x.CreateError($"{userToReplyTo.Username} hasn't sent a message in the last {messageLogLength} messages.");
                }));

            if (enumerable.Count(x => x.Author == userToReplyTo) < prevMessageIndex)
                return new ErrorResult(EmbedFactory.Create(x =>
                {
                    x.CreateError($"{userToReplyTo.Username} hasn't sent {prevMessageIndex} messages in the last {messageLogLength} messages.");
                }));

            var messageToReplyTo = enumerable.Where(x => x.Author == userToReplyTo).ToArray()[prevMessageIndex - (userToReplyTo == Context.User ? 0 : 1)];

            if (messageToReplyTo.Attachments.Count > 0 || messageToReplyTo.Embeds.Count > 0)
                return new ErrorResult(EmbedFactory.Create(x =>
                {
                    x.CreateError("Sadly, I'm unable to handle replying to messages containing attachments or embeds... sorry 😕");
                }));

            return new InfoResult(EmbedFactory.Create(x =>
            {
                x.WithColor(Color.Purple);
                x.WithDescription(userToReplyTo.Mention);
                x.AddField("Original:", messageToReplyTo.Content);
                x.AddField("Reply:", reply);
            }));
        }

        [Command("Reply"), Summary("Send a message, with a specific message you want to reply to embedded, using the message's ID. DOES NOT WORK WITH ATTACHMENTS OR OTHER EMBEDS!")]
        public async Task<RuntimeResult> Reply(ulong messageId, [Remainder] string reply)
        {
            if (!(Context.Channel is ITextChannel currentChannel))
                return new ErrorResult(EmbedFactory.Create(x =>
                {
                    x.CreateError("Sorry, was unable to cast the current channel to ITextChannel");
                }));

            IMessage message = await currentChannel.GetMessageAsync(messageId);

            if (message is null)
                return new ErrorResult(EmbedFactory.Create(x =>
                {
                    x.CreateError("Sorry, was unable to find the requested message");
                }));

            if (message.Attachments.Count > 0 || message.Embeds.Count > 0)
                return new ErrorResult(EmbedFactory.Create(x =>
                {
                    x.CreateError("Sadly, I'm unable to handle replying to messages containing attachments or embeds... sorry 😕");
                }));

            return new InfoResult(EmbedFactory.Create(x =>
            {
                x.WithColor(Color.Purple);
                x.WithDescription(message.Author.Mention);
                x.AddField("Original: ", message.Content);
                x.AddField("Reply: ", reply);
            }));
        }
    }
}
