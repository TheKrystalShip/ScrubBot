using System;
using System.Threading.Tasks;

namespace ScrubBot.Tools
{
    public static class Logger
    {
        public static Task Log<T>(T value)
        {
            if (!value.ToString().Contains("OpCode"))
            {
                Console.WriteLine(value.ToString());
            }

            return Task.CompletedTask;
        }
    }
}
