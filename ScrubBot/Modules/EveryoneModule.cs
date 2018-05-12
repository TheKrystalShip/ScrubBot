using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using ScrubBot.Database;
using ScrubBot.Database.Models;

namespace ScrubBot.Modules
{
    public class EveryoneModule : ModuleBase<SocketCommandContext>
    {
        private CommandService _commandService;

        public EveryoneModule(CommandService commandService) => Initialize(commandService);

        private void Initialize(CommandService commandService) => _commandService = commandService;

        [Command("Info"), Alias("BotInfo"), Summary("Display info about the bot.")]
        public async Task Info()
        {
            DatabaseContext db = new DatabaseContext();

            if (!GetGuild(db, out Guild guild))
            {
                EmbedBuilder errorEmbed = new EmbedBuilder { Color = Color.Red, Title = "ERROR", Description = "Current guild was not found in the database...\nAborting operation" };
                await ReplyAsync("", false, errorEmbed.Build());
                return;
            }

            EmbedBuilder embed = new EmbedBuilder { Color = Color.Purple, Title = "Bot Info" };
            embed.AddField("Server:", (guild.Name ?? "null") + "\n");

            if (guild.AuditChannelId != null)
            {
                var auditChannel = Context.Guild.GetChannel(Convert.ToUInt64(guild.AuditChannelId)) as SocketTextChannel;
                embed.AddField("Audit Channel:", (auditChannel != null ? auditChannel.Mention : "Invalid channel!") + "\n");
            }
            else
            {
                embed.AddField("Audit Channel:", "null\n");
            }

            embed.AddField("Char prefix:", (guild.CharPrefix != null ? $"' {guild.CharPrefix} '" : "null") + "\n");
            embed.AddField("String prefix:", (guild.StringPrefix != null ? $"'{guild.StringPrefix}'" : "null") + "\n");

            await ReplyAsync("", false, embed.Build());
        }

        [Command("Avatar"), Summary("Post an image of your avatar.")]
        public async Task Avatar(bool sendAsDM = true)
        {
            try
            {
                var imageEmbed = new EmbedBuilder { Color = Color.Purple, Title = $"Avatar: {Context.User}", ImageUrl = Context.User.GetAvatarUrl() }.Build();
                if (sendAsDM)
                {
                    var channel = await Context.User.GetOrCreateDMChannelAsync();
                    await channel.SendMessageAsync("", false, imageEmbed);
                }
                else
                    await ReplyAsync("", false, imageEmbed);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        [Command("Avatar"), Summary("Post an image of a specific specificUser.")]
        public async Task Avatar(SocketUser specificUser, bool sendAsDM = true)
        {
            try
            {
                var imageEmbed = new EmbedBuilder { Color = Color.Purple, Title = $"Avatar: {specificUser}", ImageUrl = specificUser.GetAvatarUrl() }.Build();
                if (sendAsDM)
                {
                    var channel = await Context.User.GetOrCreateDMChannelAsync();
                    await channel.SendMessageAsync("", false, imageEmbed);
                }
                else
                await ReplyAsync("", false, imageEmbed);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        [Command("Help")]
        public async Task Help()
        {
            List<CommandInfo> commands = _commandService.Commands.ToList();
            EmbedBuilder embed = new EmbedBuilder { Color = Color.Purple, Title = "Command list" };

            foreach (var command in commands)
            {
                if (command.Name == "Help") continue;

                if (command.Module.Name == typeof(OwnerModule).Name) continue;

                string embedFieldText = command.Summary;

                if (command.Parameters.Count > 0)
                    embedFieldText = command.Parameters.Aggregate(embedFieldText + $"\nParameter" + (command.Parameters.Count > 1 ? "s:" : ":") + "\t\t", (current, param) => current + $"{param}\t\t");

                embed.AddField($"{command.Name} ({command.Module.Name.Replace("Module", "")})", embedFieldText);
            }

            await ReplyAsync("", false, embed.Build());
        }

        private bool GetGuild(DatabaseContext dbContext, out Guild outGuild)
        {
            string guildId = Context.Guild.Id.ToString();
            Guild localGuild = dbContext.Guilds.FirstOrDefault(x => x.Id == guildId);

            if (localGuild == null)
            {
                outGuild = null;
                return false;
            }

            outGuild = localGuild;
            return true;
        }

        private async Task OnGuildNotFound() => await ReplyAsync($"```Current guild was not found in the database...\nAborting operation```");
    }
}