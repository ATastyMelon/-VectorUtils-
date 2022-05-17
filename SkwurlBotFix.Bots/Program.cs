using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace SkwurlBotFix.Bots
{
    class Program
    {
        static void Main(string[] args)
        {
            var bot = new Bot();
            bot.RunAsync().GetAwaiter().GetResult();
        }
    }
}
