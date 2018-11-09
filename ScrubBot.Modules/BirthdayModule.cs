using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using ScrubBot.Domain;
using ScrubBot.Extensions;

namespace ScrubBot.Modules
{
    public class BirthdayModule : Module
    {
        //public BirthdayModule() { }

        [Command("SetBirthday"), Summary("Let me know of your birthday! : D")]
        public async Task SetBirthday(DateTime birthday)
        {
            User.Birthday = birthday;
            await ReplyAsync(string.Empty,
                false,
                new EmbedBuilder().CreateSuccess($"Successfully set the birthday for {Context.User.Username} to {birthday:dddd, dd MMMM yyyy}"));
        }

        [Command("ShowBirthdays"), Summary("Show all birthdays in a specific month")]
        public async Task ShowBirthdays(int month)
        {
            if (month < 1 || month > 12)
            {
                await ReplyAsync($"Cannot parse {month} as a month. Please type this command, followed by a number between 1 and 12!");
                return;
            }

            List<User> birthdayBois = Database.Users.Where(x => Context.Guild.GetUser(x.Id) != null && x.Birthday.Month == month).ToList();

            var today = DateTime.UtcNow;
            var birthdayDate = DateTime.Parse($"{today.Day}-{month}-{today.Year}");
            var birthdayMonth = birthdayDate.ToString("MMMMM");

            if (birthdayBois.Count is 0)
            {
                await ReplyAsync(string.Empty,
                                 false,
                                 new EmbedBuilder().CreateError($"Sadly, I know no one with a birthday in {birthdayMonth}...\nEither no one in this server has his/her birthday that month, or they forgot to tell me!"));
                return;
            }

            string birthdayList = birthdayBois.Aggregate(string.Empty, (current, birthdayBoi) => current + $"{birthdayBoi.Username} ({birthdayBoi.Birthday:dddd, dd MMMM yyyy})\n");

            await ReplyAsync(string.Empty, false, new EmbedBuilder().CreateMessage($"Birthdays in {birthdayMonth}", birthdayList));
        }
    }
}
