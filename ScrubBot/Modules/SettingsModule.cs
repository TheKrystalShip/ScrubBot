using Discord;
using Discord.Commands;
using Discord.WebSocket;

using ScrubBot.Core;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrubBot.Modules
{
    public class SettingsModule : Module
    {
        public SettingsModule()
        {

        }

        [Command("Info"), Alias("BotInfo"), Summary("Display info about the bot.")]
        public async Task Info()
        {
            EmbedBuilder embedBuilder = new EmbedBuilder { Color = Color.Purple, Title = "Bot Info" };
            embedBuilder.AddField("Server:", (Guild.Name ?? "null") + "\n");

            SocketTextChannel auditChannel = Context.Guild.GetChannel(Guild.AuditChannelId) as SocketTextChannel;
            embedBuilder.AddField("Audit Channel:", (auditChannel != null ? auditChannel.Mention : "Invalid channel!") + "\n");
            embedBuilder.AddField("String prefix:", (Guild.Prefix != null ? $"'{Guild.Prefix}'" : "null") + "\n");

            await ReplyAsync(embedBuilder);
        }

        [Command("Help")]
        public async Task Help()
        {
            List<CommandInfo> commands = CommandService.Commands.ToList();
            EmbedBuilder embedBuilder = new EmbedBuilder { Color = Color.Purple, Title = "Command list" };

            foreach (CommandInfo command in commands)
            {
                if (command.Name == "Help") continue;

                string embedFieldText = command.Summary ?? "No description available\n";

                if (command.Parameters.Count > 0)
                    embedFieldText = command.Parameters.Aggregate(embedFieldText, (current, param) => current + $"\nParameters:\t{param.Type.Name} {param}\t");

                embedBuilder.AddField($"{command.Name} ({command.Module.Name.Replace("Module", "")})", embedFieldText);
            }

            await ReplyAsync(embedBuilder);
        }
    }
}
