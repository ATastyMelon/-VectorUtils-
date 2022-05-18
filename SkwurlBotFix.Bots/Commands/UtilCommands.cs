using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace SkwurlBotFix.Bots.Commands
{
    class UtilCommands : BaseCommandModule
    {
        [Command("clear")]
        [Aliases("c")]
        [RequireUserPermissions(DSharpPlus.Permissions.Administrator)]
        [RequireBotPermissions(DSharpPlus.Permissions.Administrator)]
        public async Task ClearMessages(CommandContext ctx, int amount)
        {
            var messages = await ctx.Channel.GetMessagesAsync((amount + 1));
            
            await ctx.Channel.DeleteMessagesAsync(messages);
        }

        [Command("ban")]
        [Aliases("b")]
        [RequireUserPermissions(DSharpPlus.Permissions.BanMembers)]
        [RequireBotPermissions(DSharpPlus.Permissions.BanMembers)]
        public async Task BanMembers(CommandContext ctx, DiscordMember member, string reason)
        {
            await ctx.TriggerTypingAsync();
            DiscordGuild guild = member.Guild;

            try
            {
                await guild.BanMemberAsync(member, 1, reason);
                await ctx.RespondAsync($"User {member.Username}#{member.Discriminator} was banned by {ctx.User.Username}");
            }
            catch (Exception)
            {
                await ctx.RespondAsync($"User {member.Username} cannot be banned");
            }
        }

        [Command("Kick")]
        [Aliases("k")]
        [RequireUserPermissions(DSharpPlus.Permissions.KickMembers)]
        [RequireBotPermissions(DSharpPlus.Permissions.KickMembers)]
        public async Task KickMembers(CommandContext ctx, DiscordMember member, string reason)
        {
            await ctx.TriggerTypingAsync();

            try
            {
                await member.RemoveAsync(reason);
            }
            catch (Exception)
            {
                await ctx.RespondAsync($"User {member.Username} cannot be kicked");
            }
        }

        [Command("nick")]
        [RequirePermissions(DSharpPlus.Permissions.ManageNicknames)]
        public async Task ChangeNickname(CommandContext ctx, DiscordMember member, string newName)
        {
            await ctx.TriggerTypingAsync();

            try
            {
                await member.ModifyAsync(x =>
                {
                    x.Nickname = newName;
                    x.AuditLogReason = $"Changed by {ctx.User.Username} ({ctx.User.Id})";
                });

                var emoji = DiscordEmoji.FromName(ctx.Client, ":+1:");
                await ctx.RespondAsync(emoji);
            }
            catch (Exception)
            {
                var emoji = DiscordEmoji.FromName(ctx.Client, ":-1:");
                await ctx.RespondAsync(emoji);
            }
        }
    }
}
