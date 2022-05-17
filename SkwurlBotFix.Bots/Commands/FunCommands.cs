using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkwurlBotFix.Bots.Commands
{
    public class FunCommands : BaseCommandModule
    {
        [Command("pp")]
        [Hidden]
        public async Task PP(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("AYOO").ConfigureAwait(false);
        }

        [Command("add")]
        [Hidden]
        [Description("Adds two numbers together")]
        public async Task Add(CommandContext ctx, int numOne, int numTwo)
        {
            DiscordEmbedBuilder embed = new DiscordEmbedBuilder
            {
                Color = DiscordColor.SpringGreen,
                Description = (numOne + numTwo).ToString(),
                Title = "Answer"
            };

            await ctx.Channel
                .SendMessageAsync(embed: embed)
                .ConfigureAwait(false);
        }

        [Command("response")]
        [Hidden]
        public async Task RespondMessage(CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivity();

            var message = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel).ConfigureAwait(false);

            await ctx.Channel.SendMessageAsync(message.Result.Content);
        }

        [Command("patrick")]
        public async Task Patrick(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("https://tenor.com/view/butt-i-luv-patrick-yea-gif-13484694").ConfigureAwait(false);
        }

        [Command("lore")]
        [RequireRoles(RoleCheckMode.Any, "Lore Chad")]
        public async Task LoreChad(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("HE IS THE LORE CHAD").ConfigureAwait(false);
        }

        [Command("bungus")]
        public async Task BUNGUS(CommandContext ctx)
        {
            var x = "https://tenor.com/view/wiggly-green-mushrooms-risk-of-rain2-gif-23657855";

            await ctx.Channel.SendMessageAsync($"I LOVE BUNGUS {x}");
        }
    }
}
