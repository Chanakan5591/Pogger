using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Pogger.Events;

namespace Pogger
{
    class Program
    {
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        public async Task RunBotAsync()
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

        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
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
                var embedReply = new EmbedBuilder();
                if (result.Error != null)
                {
                    switch (result.ErrorReason)
                    {
                        case "The server responded with error 403: Forbidden":
                            embedReply.WithTitle("You do not have permission to execute this command");
                            embedReply.WithDescription("sorry, but you did not have the valid permission to execute this command :(");
                            embedReply.WithColor(Color.Red);
                            Embed EmbedReply = embedReply.Build();
                            await context.Channel.SendMessageAsync(embed: EmbedReply);
                            break;
                    }
                }
            }
        }
    }
}
