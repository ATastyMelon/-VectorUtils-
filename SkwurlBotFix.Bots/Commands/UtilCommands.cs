using System;
using System.Collections.Generic;
using System.Linq;
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
        [Description("Clears a certain amount of messages from the chat it is used in")]
        [RequireUserPermissions(DSharpPlus.Permissions.Administrator)]
        [RequireBotPermissions(DSharpPlus.Permissions.Administrator)]
        public async Task ClearMessages(CommandContext ctx, int amount)
        {
            var messages = await ctx.Channel.GetMessagesAsync((amount + 1));
            
            await ctx.Channel.DeleteMessagesAsync(messages);
        }

        [Command("ban")]
        [Aliases("b")]
        [Description("Bans a member permanently")]
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
        [Description("Kicks a member from the server")]
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
        [Description("Changes a members nickname")]
        [RequirePermissions(DSharpPlus.Permissions.ManageNicknames)]
        public async Task ChangeNickname(CommandContext ctx, DiscordMember member, params string[] newName)
        {
            await ctx.TriggerTypingAsync();

            string newString = string.Concat(newName);

            try
            {
                await member.ModifyAsync(x =>
                {
                    x.Nickname = newString;
                    x.AuditLogReason = $"Changed by {ctx.User.Username} ({ctx.User.Id})";
                });

                await ctx.RespondAsync($"{member.Username} nickname changed to {newString}");
            }
            catch (Exception)
            {
                await ctx.RespondAsync($"Unable to nickname {member.Username}");
            }
        }
    }
}
