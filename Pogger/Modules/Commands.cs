using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Pogger.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task Ping()
        {
            await ReplyAsync($"Pong: {Context.Client.Latency}ms!");
        }
        [Command("help")]
        public async Task Help()
        {
            var EmbedBuilderHelp = new EmbedBuilder();
            
            EmbedBuilderHelp.WithTitle("e");
            Embed embed = EmbedBuilderHelp.Build();
            await ReplyAsync(embed: embed);
        }
        [Command("ban")]
        [RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "You need ``ban_member`` permission!")]
        public async Task Ban(IGuildUser userToBan = null, [Remainder] string reason = null)
        {
            if (userToBan == null)
            {
                await ReplyAsync("Please specify a user!");
                return;
            }
            
            if(reason == null) reason = "Not specified";
            IGuildUser user = null;

            IGuildUser userInEmbed = null;
            ulong userID;
            if (ulong.TryParse(userToBan.ToString(), out userID))
            {
                user = Context.Guild.GetUser(userID);
                if(user.Id == Context.User.Id) {
                    await ReplyAsync("You cannot ban yourself, sorry!");
                    return;
                }
                if(user.Id == 720579758026522667) {
                    await ReplyAsync("I cannot ban myself, sorry!");
                    return;
                }
                await Context.Guild.AddBanAsync(user, 7, reason);
                userInEmbed = user;
            }
            else
            {
                if(userToBan.Id == Context.User.Id) {
                    await ReplyAsync("You cannot ban yourself, sorry!");
                    return;
                }
                if(userToBan.Id == 720579758026522667) {
                    await ReplyAsync("I cannot ban myself, sorry!");
                    return;
                }
                await Context.Guild.AddBanAsync(userToBan, 7, reason);
                userInEmbed = userToBan;
            }

            var EmbedBuilderReply = new EmbedBuilder();
            var EmbedBuilderDM = new EmbedBuilder();
            
            EmbedBuilderReply.WithTitle($":white_check_mark: {userInEmbed.Username}#{userInEmbed.Discriminator} was banned");
            EmbedBuilderReply.WithDescription($"User {userInEmbed.Mention} has been successfully banned");
            EmbedBuilderReply.AddField("Moderator", Context.User.Mention, true);
            EmbedBuilderReply.AddField("Reason", reason, true);
            Embed embed = EmbedBuilderReply.Build();
            await ReplyAsync(embed: embed);
            EmbedBuilderDM.WithTitle($"You have been banned from **{Context.Guild.Name}**!");
            EmbedBuilderDM.AddField("Reason", reason, true);
            Embed embedDM = EmbedBuilderDM.Build();
            await userInEmbed.SendMessageAsync(embed: embedDM);

            ITextChannel logChannel = Context.Client.GetChannel(Global.ModLogChannel) as ITextChannel;
            var EmbedBuilderLog = new EmbedBuilder();
            
            EmbedBuilderLog.WithTitle($"{userInEmbed.Username}#{userInEmbed.Discriminator} was banned");
            EmbedBuilderLog.AddField("Moderator", Context.User.Mention, true);
            EmbedBuilderLog.AddField("Reason", reason, true);
                
            Embed embedLog = EmbedBuilderLog.Build();
            await logChannel.SendMessageAsync(embed: embedLog);
        }
        
        [Command("kick")]
        [RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "You need ``kick_member`` permission!")]
        public async Task Kick(IGuildUser user = null, [Remainder] string reason = null)
        {
            if (user == null)
            {
                await ReplyAsync("Please specify a user!");
                return;
            }
            if(reason == null) reason = "Not specified";
            if(user.Id == Context.User.Id) {
                await ReplyAsync("You cannot kick yourself, sorry!");
                return;
            }
            if(user.Id == 720579758026522667) {
                await ReplyAsync("I cannot kick myself, sorry!");
                return;
            }
            await user.KickAsync(reason);
            
            var EmbedBuilderReply = new EmbedBuilder();
            var EmbedBuilderDM = new EmbedBuilder();
            
            EmbedBuilderReply.WithTitle($":white_check_mark: {user.Mention} was kicked");
            EmbedBuilderReply.AddField("Reason", reason, true);
            Embed embed = EmbedBuilderReply.Build();
            await ReplyAsync(embed: embed);
            EmbedBuilderDM.WithTitle($"You have been kicked from {Context.Guild.Name}!");
            EmbedBuilderDM.AddField("Reason", reason, true);
            Embed embedDM = EmbedBuilderDM.Build();
            await user.SendMessageAsync(embed: embedDM);
            
            ITextChannel logChannel = Context.Client.GetChannel(Global.ModLogChannel) as ITextChannel;
            var EmbedBuilderLog = new EmbedBuilder();
            
            EmbedBuilderLog.WithTitle($"{user.Mention} was kicked");
            EmbedBuilderLog.AddField("Moderator", Context.User.Mention, true);
            EmbedBuilderLog.AddField("Reason", reason, true);
                
            Embed embedLog = EmbedBuilderLog.Build();
            await logChannel.SendMessageAsync(embed: embedLog);
        }
    }
}
