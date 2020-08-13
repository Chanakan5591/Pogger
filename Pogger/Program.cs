using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Pogger.Events;

namespace Pogger
{
    internal class Program
    {
        private static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        private async Task RunBotAsync()
        {
            Global.ReadConfig();
            UserJoinEvent onUserJoinEvent = new UserJoinEvent();
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug,
                AlwaysDownloadUsers = true,
            });
            _commands = new CommandService();

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            _client.Log += _client_Log;
            _client.UserJoined += onUserJoinEvent.OnUserJoin;
            _client.ReactionAdded += HandleHelpMsg;

            await _client.SetGameAsync(Global.Status, null, ActivityType.Playing);
            await _client.SetStatusAsync(UserStatus.Online);
            await RegisterCommandsAsync();
            await _client.LoginAsync(TokenType.Bot, Global.Token);
            await _client.StartAsync();
            await Task.Delay(-1);

        }

        private async Task _client_Log(LogMessage msg)
        {
           
            if (msg.Message == null)
                return;
            if (!msg.Message.StartsWith("Received Dispatch"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[Svt: {msg.Severity} Src: {msg.Source} Ex: {msg.Exception}] - " + msg.Message);
            }
        }

        private async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleHelpMsg(Cacheable<IUserMessage, ulong> msg, ISocketMessageChannel msgChannel, SocketReaction reactEmote)
        {
            if (reactEmote.User.Value.IsBot) return;
            var toolEmote = new Emoji("🛠️");
            if (!Global.HelpMsgObj.Contains(msg.Id)) return;
            if (reactEmote.Emote.Equals(toolEmote))
            {
                var context = new SocketCommandContext(_client, reactEmote.Message.Value);
                await context.Channel.SendMessageAsync("Tool Yes");
            }
        }
        
        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            if (message == null) return;
            var context = new SocketCommandContext(_client, message);
            if (message.Author.IsBot) return;

            int argPos = 0;
            if (message.HasStringPrefix(Global.Prefix, ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (result.Error != null)
                {
                    switch (result.ErrorReason)
                    {
                        case "The server responded with error 403: Forbidden":
                            var embedReply = new EmbedBuilder();
                            embedReply.WithTitle("You do not have permission to execute this command");
                            embedReply.WithDescription("sorry, but you did not have the valid permission to execute this command :(");
                            embedReply.WithColor(Color.Red);
                            Embed EmbedReply = embedReply.Build();
                            await context.Channel.SendMessageAsync(embed: EmbedReply);
                            break;
                        case "User not found.":
                            var embedReplyU = new EmbedBuilder();
                            embedReplyU.WithTitle("User Not Found");
                            embedReplyU.WithDescription("sorry, but the ID you provided is probably invalid or the user is not in the server.");
                            embedReplyU.WithColor(Color.Red);
                            Embed EmbedReplyU = embedReplyU.Build();
                            await context.Channel.SendMessageAsync(embed: EmbedReplyU);
                            break;
                        case "no_perm":
                            var embedReplyUU = new EmbedBuilder();
                            embedReplyUU.WithTitle("You do not have permission to execute this command");
                            embedReplyUU.WithDescription("sorry, but you did not have the valid permission to execute this command :(");
                            embedReplyUU.WithColor(Color.Red);
                            Embed EmbedReplyUU = embedReplyUU.Build();
                            await context.Channel.SendMessageAsync(embed: EmbedReplyUU);
                            break;
                        case "Unknown command.":
                            break;
                        default:
                            var embedReply2 = new EmbedBuilder();
                            embedReply2.WithTitle("Uh oh! This shouldn't happen");
                            embedReply2.WithDescription("I've dm'ed Chanakan the errors and it should be fix as soon as he read the errors.");
                            embedReply2.WithColor(Color.Red);
                            Embed EmbedReply2 = embedReply2.Build();
                            await context.Channel.SendMessageAsync(embed: EmbedReply2);
                            var embedDM = new EmbedBuilder();
                            embedDM.WithTitle($"Error occurred in {context.Guild.Name}");
                            embedDM.WithDescription($"The errors is: `{result.ErrorReason}`");
                            Embed EmbedDM = embedDM.Build();
                            SocketUser ChanakanUser = _client.GetUser(456961943505338369);
                            await ChanakanUser.SendMessageAsync(embed: EmbedDM);
                            break;
                    }
                }
            }
        }
    }
}
