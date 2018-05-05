using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

using ScrubBot.Data;

namespace ScrubBot.Modules
{
    [RequireUserPermission(GuildPermission.Administrator)]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        [Command("UrMomGay")] public async Task UrMomGay() => await ReplyAsync($"{Context.Message.Author.Mention} No u");

        [Command("FuckOff")] public async Task FuckOff() => await ReplyAsync("Na fam");

        [Command("ShowSettings")] public async Task ShowSettings()
        {
            DatabaseContext db = new DatabaseContext();
            string guildId = Context.Guild.Id.ToString();
            Guild guild = db.Guilds.FirstOrDefault(x => x.Id == guildId);

            await ReplyAsync($"Server:\t\t{guild.Name}\n" +
                             $"Char prefix:\t\t{guild.CharPrefix}\n" +
                             $"String prefix:\t\t{guild.StringPrefix}");
        }
        
        [Command("ChangeCharPrefix")]
        public async Task ChengeCharPrefix(string newPrefix)
        {
            DatabaseContext db = new DatabaseContext();

            string guildId = Context.Guild.Id.ToString();
            Guild guild = db.Guilds.FirstOrDefault(x => x.Id == guildId);

            if (guild is null) return;

            await ReplyAsync($"`Changing Command Char Prefix from {guild.CharPrefix} to {newPrefix}`");
            guild.CharPrefix = newPrefix;
            db.Guilds.Update(guild);
            await db.SaveChangesAsync();
        }

        [Command("ChangeStringPrefix")]
        public async Task ChengeStringPrefix(string newPrefix)
        {
            DatabaseContext db = new DatabaseContext();

            string guildId = Context.Guild.Id.ToString();
            Guild guild = db.Guilds.FirstOrDefault(x => x.Id == guildId);

            if (guild is null) return;

            guild.StringPrefix = newPrefix;
            db.Guilds.Update(guild);
            await db.SaveChangesAsync();
        }
    }
}