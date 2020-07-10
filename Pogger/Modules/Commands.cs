using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Pogger.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task Ping()
        {
            await ReplyAsync($"Pong: {Context.Client.Latency}ms!");
        }
        [Command("ban")]
        [RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "You need ``ban_member`` permission!")]
        public async Task Ban(IGuildUser user = null, [Remainder] string reason = null)
        {
            if (user == null)
            {
                await ReplyAsync("Please specify a user!");
                return;
            }
            if(reason == null) reason = "Not specified";
            if(user.Id == Context.User.Id) {
                await ReplyAsync("You cannot ban yourself, sorry!");
                return;
            }
            await Context.Guild.AddBanAsync(user, 1, reason);
            
            var EmbedBuilderReply = new EmbedBuilder();
            
            EmbedBuilderReply.WithTitle($":white_check_mark: {user.Mention} was banned");
            EmbedBuilderReply.AddField("Reason", reason, true);
            Embed embed = EmbedBuilderReply.Build();
            await ReplyAsync(embed: embed);
            
            ITextChannel logChannel = Context.Client.GetChannel(Global.ModLogChannel) as ITextChannel;
            var EmbedBuilderLog = new EmbedBuilder();
            
            EmbedBuilderLog.WithTitle($"{user.Mention} was banned");
            EmbedBuilderLog.AddField("Moderator", Context.User.Mention, true);
            EmbedBuilderLog.AddField("Reason", reason, true);
                
            Embed embedLog = EmbedBuilderLog.Build();
            await logChannel.SendMessageAsync(embed: embedLog);
        }
    }
}
