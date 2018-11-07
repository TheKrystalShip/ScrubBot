using System;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ScrubBot.Domain;
using ScrubBot.Extensions;

namespace ScrubBot.Modules
{
    public class UserModule : Module
    {
        public UserModule()
        {

        }

        [Command("Info"), Alias("BotInfo"), Summary("Display info about the bot.")]
        public async Task Info()
        {
            EmbedBuilder embedBuilder = new EmbedBuilder { Color = Color.Purple, Title = "Bot Info" };
            embedBuilder.AddField("Server:", (Guild.Name ?? "null") + "\n");
            embedBuilder.ThumbnailUrl = Guild.IconUrl;

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

        [Command("SetBirthday")]
        public async Task SetBirthday(DateTime birthday)
        {
            User.Birthday = birthday;
            await ReplyAsync(string.Empty,
                false,
                new EmbedBuilder().CreateSuccess($"Successfully set the birthday for {Context.User.Username} to {birthday:dddd, dd MMMM yyyy}"));
        }

        [Command("ShowBirthdays")]
        public async Task ShowBirthdays(int month)
        {
            if (month < 1 || month > 12)
            {
                await ReplyAsync($"Cannot parse {month} as a month. Please type this command, followed by a number between 1 and 12!");
                return;
            }

            List<User> birthdayBois = Database.Users.Where(x => x.Guilds.Contains(y => y.Id == Context.Guild.Id) && x.Birthday.Month == month)
                .ToList();

            var today = DateTime.UtcNow;
            var birthdayMonth = DateTime.Parse($"{today.Day}-{month}-{today.Year}").ToString("MMMMM");

            if (birthdayBois.Count is 0)
            {
                await ReplyAsync(string.Empty,
                                 false,
                                 new EmbedBuilder().CreateError($"Sadly, I know no one with a birthday in {birthdayMonth}...\nEither no one in this server has his/her birthday that month, or they forgot to tell me!"));
                return;
            }

            //foreach (var birthdayBoi in birthdayBois)
            //{
            //    birthdayList += $"{birthdayBoi.Username} ({birthdayBoi.Birthday:dddd, dd MMMM yyyy})";
            //}

            string birthdayList = birthdayBois.Aggregate(string.Empty, (current, birthdayBoi) => current + $"{birthdayBoi.Username} ({birthdayBoi.Birthday:dddd, dd MMMM yyyy})");

            await ReplyAsync(string.Empty, false, new EmbedBuilder().CreateMessage($"Birthdays in {birthdayMonth}", birthdayList));
        }
    }
}