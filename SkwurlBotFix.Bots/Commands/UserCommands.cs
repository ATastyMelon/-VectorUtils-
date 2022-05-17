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
using static DSharpPlus.Entities.DiscordEmbedBuilder;
using Microsoft.EntityFrameworkCore;

namespace SkwurlBotFix.Bots.Commands
{
    class UserCommands : BaseCommandModule
    {
        

        [Command("vs")]
        [Hidden]
        [RequireRoles(RoleCheckMode.Any, "Verified")]
        public async Task Join(CommandContext ctx)
        {
            var joinEmbed = new DiscordEmbedBuilder
            {
                Title = "Do you want to get the Video Skwurl rank?",
                Thumbnail = new EmbedThumbnail() { Url = ctx.Client.CurrentUser.AvatarUrl },
                Color = DiscordColor.Black
            };

            var joinMessage = await ctx.Channel.SendMessageAsync(embed: joinEmbed).ConfigureAwait(false);

            var greenSquare = DiscordEmoji.FromName(ctx.Client, ":green_square:");
            var redSquare = DiscordEmoji.FromName(ctx.Client, ":red_square:");

            await joinMessage.CreateReactionAsync(greenSquare).ConfigureAwait(false);
            await joinMessage.CreateReactionAsync(redSquare).ConfigureAwait(false);

            var interactivity = ctx.Client.GetInteractivity();

            var reactionResult = await interactivity.WaitForReactionAsync(
                x => x.Message == joinMessage &&
                x.User == ctx.User &&
                (x.Emoji == greenSquare || x.Emoji == redSquare)).ConfigureAwait(false);

            if (reactionResult.Result.Emoji == greenSquare)
            {
                var role = ctx.Guild.GetRole(959147832986697788);
                await ctx.Member.GrantRoleAsync(role).ConfigureAwait(false);
            }
            else if (reactionResult.Result.Emoji == redSquare)
            {
                var role = ctx.Guild.GetRole(959147832986697788);
                await ctx.Member.RevokeRoleAsync(role).ConfigureAwait(false);
            }
        }

        [Command("poll")]
        [RequireRoles(RoleCheckMode.Any, "Moderator", "Admin", "Melon")]
        public async Task Poll(CommandContext ctx, params string[] content)
        {
            var embed = new DiscordEmbedBuilder
            {
                Title = "Poll",
                Description = string.Join(" ", content),
                Color = DiscordColor.Black
            };

            

            await ctx.Channel.SendMessageAsync("POLL TIME");
            var pollMessage = await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

            var greenSquare = DiscordEmoji.FromName(ctx.Client, ":green_square:");
            var redSquare = DiscordEmoji.FromName(ctx.Client, ":red_square:");

            await pollMessage.CreateReactionAsync(greenSquare).ConfigureAwait(false);
            await pollMessage.CreateReactionAsync(redSquare).ConfigureAwait(false);
        }
    }
}
