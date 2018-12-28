using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using ScrubBot.Database.Domain;
using ScrubBot.Tools;

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
            SocketTextChannel auditChannel = null;
            if (Guild.AuditChannelId != null)
            {
                auditChannel = Context.Guild.GetChannel((ulong)Guild.AuditChannelId) as SocketTextChannel;
            }

            Embed embed = EmbedFactory.Create(builder => {
                builder.WithColor(Color.Purple);
                builder.WithTitle("Bot info");
                builder.ThumbnailUrl = Guild.IconUrl;
                builder.AddField("Server:", (Guild.Name ?? "null"));
                builder.AddField("Audit Channel:", (auditChannel != null ? auditChannel.Mention : "Invalid channel!"));
                builder.AddField("String prefix:", (Guild.Prefix != null ? $"'{Guild.Prefix}'" : "null"));
            });

            await ReplyAsync(embed);
        }

        [Command("Help")]
        public async Task Help()
        {
            List<CommandInfo> commands = CommandService.Commands.ToList();

            Embed embed = EmbedFactory.Create(builder => {
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
    }
}
