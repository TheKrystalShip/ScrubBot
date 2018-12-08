using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Discord.Commands;

using ScrubBot.Core.Commands;
using ScrubBot.Database.Domain;

namespace ScrubBot.Modules
{
    public class BirthdayModule : Module
    {
        [Command("SetBirthday"), Summary("Let me know of your birthday! :D")]
        public async Task<RuntimeResult> SetBirthday(DateTime birthday)
        {
            User.Birthday = birthday;
            return Result.Success($"Successfully set the birthday for {Context.User.Username} to {birthday:dddd, dd MMMM yyyy}");
        }

        [Command("ShowBirthdays"), Summary("Show all birthdays in a specific month")]
        public async Task<RuntimeResult> ShowBirthdays(int month)
        {
            if (month < 1 || month > 12)
            {
                return Result.Error(CommandError.ParseFailed, $"Cannot parse {month} as a month. Please type this command, followed by a number between 1 and 12!");
            }

            List<User> birthdayBois = Database.Users.Where(x => Context.Guild.GetUser(x.Id) != null && x.Birthday.Month == month).ToList();

            DateTime today = DateTime.UtcNow;
            DateTime birthdayDate = DateTime.Parse($"{today.Day}-{month}-{today.Year}");
            string birthdayMonth = birthdayDate.ToString("MMMMM");

            if (birthdayBois.Count is 0)
            {
                return Result.Error($"Sadly, I know no one with a birthday in {birthdayMonth}...\nEither no one in this server has his/her birthday that month, or they forgot to tell me!");
            }

            string birthdayList = birthdayBois.Aggregate(string.Empty, (current, birthdayBoi) => current + $"{birthdayBoi.Username} ({birthdayBoi.Birthday:dddd, dd MMMM yyyy})\n");

            return Result.Info(birthdayList);
        }
    }
}
