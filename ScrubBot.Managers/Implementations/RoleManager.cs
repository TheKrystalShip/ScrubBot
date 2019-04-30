using System;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

using ScrubBot.Extensions;

namespace ScrubBot.Managers
{
    public class RoleManager : IRoleManager
    {
        public RoleManager()
        {

        }

        public async Task OnRoleCreatedAsync(SocketRole role)
        {
            Console.WriteLine(new LogMessage(LogSeverity.Info, GetType().Name, $"Role: {role.Name} has been created in guild: {role.Guild.Name}"));

            await Task.CompletedTask;
        }

        public async Task OnRoleDeletedAsync(SocketRole role)
        {
            Console.WriteLine(new LogMessage(LogSeverity.Warning, GetType().Name, $"Role: {role.Name} has been deleted in guild: {role.Guild.Name}"));

            await Task.CompletedTask;
        }

        public async Task OnRoleUpdatedAsync(SocketRole before, SocketRole after)
        {
            Console.WriteLine(new LogMessage(LogSeverity.Info, GetType().Name, $"Role: {before.Name} updated"));
            Console.WriteLine(before.Compare(after).BuildString());

            await Task.CompletedTask;
        }
    }
}
