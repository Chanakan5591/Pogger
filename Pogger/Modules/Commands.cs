using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Pogger.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("ping"), Summary("Get the ping of the bot")]
        public async Task Ping()
        {
            await ReplyAsync($"Pong: {Context.Client.Latency}ms!");
        }
        [Command("8ball"), Summary("Ask the question to magic 8ball!")]
        public async Task EightBall([Remainder]String _)
        {
            List<string> responseList = new List<string> {"It is certain", "Without a doubt", "You may rely on it", "Yes definitely", "It is decidedly so", "As I see it, yes", "Most likely", "Yes", "Outlook good", "Signs point to yes", "Reply hazy try again", "Better not tell you now", "Ask again later", "Cannot predict now", "Concentrate and ask again", "Don't count on it", "Outlook not so good", "My sources say no", "Very doubtful", "My reply is no"};
            Random rnd = new Random();
            int randomItem = rnd.Next(responseList.Count);
            await ReplyAsync(responseList[randomItem]);
        }
        [Command("ban"), Summary("Usage:\n[PREFIX]ban <mention-user-or-user-id> <reason>")]
        [RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "no_perm")]
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
        
        [Command("kick"), Summary("Usage:\n[PREFIX]kick <mention-user-or-user-id> <reason>")]
        [RequireUserPermission(GuildPermission.KickMembers, ErrorMessage = "no_perm")]
        public async Task Kick(IGuildUser userToKick = null, [Remainder] string reason = null)
        {
            if (userToKick == null)
            {
                await ReplyAsync("Please specify a user!");
                return;
            }
            
            if(reason == null) reason = "Not specified";
            IGuildUser user = null;

            IGuildUser userInEmbed = null;
            ulong userID;
            if (ulong.TryParse(userToKick.ToString(), out userID))
            {
                user = Context.Guild.GetUser(userID);
                if(user.Id == Context.User.Id) {
                    await ReplyAsync("You cannot kick yourself, sorry!");
                    return;
                }
                if(user.Id == 720579758026522667) {
                    await ReplyAsync("I cannot kick myself, sorry!");
                    return;
                }

                await user.KickAsync(reason);
                userInEmbed = user;
            }
            else
            {
                if(userToKick.Id == Context.User.Id) {
                    await ReplyAsync("You cannot kick yourself, sorry!");
                    return;
                }
                if(userToKick.Id == 720579758026522667) {
                    await ReplyAsync("I cannot kick myself, sorry!");
                    return;
                }

                await userToKick.KickAsync(reason);
                userInEmbed = userToKick;
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
    }
}
