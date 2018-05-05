using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

using ScrubBot.Database;
using ScrubBot.Database.Models;
using ScrubBot.Handlers;

namespace ScrubBot.Modules
{
    [RequireUserPermission(GuildPermission.Administrator)]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        [Command("UrMomGay")] public async Task UrMomGay() => await ReplyAsync($"{Context.Message.Author.Mention} No u");

        [Command("Help")] public async Task Help()
        {
            DatabaseContext db = new DatabaseContext();
            string guildId = Context.Guild.Id.ToString();
            Guild guild = db.Guilds.FirstOrDefault(x => x.Id == guildId);

            EmbedBuilder embed = new EmbedBuilder();
            embed.Color = Color.DarkBlue;
            embed.AddField("Server", guild.Name ?? "null");
            embed.AddField("Char prefix:", guild.CharPrefix ?? "null");
            embed.AddField("String prefix", guild.StringPrefix ?? "null");

            await ReplyAsync(string.Empty, false, embed.Build());
        }
        
        [Command("ChangeCharPrefix")]
        public async Task ChengeCharPrefix(string newPrefix)
        {
            DatabaseContext db = new DatabaseContext();

            string guildId = Context.Guild.Id.ToString();
            Guild guild = db.Guilds.FirstOrDefault(x => x.Id == guildId);

            if (guild is null) return;

            string old = guild.CharPrefix;
            guild.CharPrefix = newPrefix;
            db.Guilds.Update(guild);
            await db.SaveChangesAsync();
            PrefixHandler.SetCharPrefix(guildId, newPrefix);
            await ReplyAsync($"`Changing Command Char Prefix from {old} to {newPrefix}`");
        }

        [Command("ChangeStringPrefix")]
        public async Task ChengeStringPrefix([Remainder]string newPrefix)
        {
            DatabaseContext db = new DatabaseContext();

            string guildId = Context.Guild.Id.ToString();
            Guild guild = db.Guilds.FirstOrDefault(x => x.Id == guildId);

            if (guild is null) return;

            if (!newPrefix.EndsWith(" ")) newPrefix += " ";
            
            string old = guild.StringPrefix;
            guild.StringPrefix = newPrefix;
            db.Guilds.Update(guild);
            await db.SaveChangesAsync();
            PrefixHandler.SetStringPrefix(guildId, newPrefix);
            await ReplyAsync($"`Changing Command String Prefix from {old} to {newPrefix}`");
        }
    }
}