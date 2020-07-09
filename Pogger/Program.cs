using BetterCommandService;
using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace Pogger
{
    public class Program
    {
        private DiscordSocketClient _client;
        private CommandHandler _handler; 
        private HandlerService handlerService;

        public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            Global.ReadConfig();
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug,
                AlwaysDownloadUsers = true,
            }); 
            _client.Log += Log;

            await _client.LoginAsync(TokenType.Bot, Global.Token);
            await _client.StartAsync();

            Global.Client = _client;

            CustomCommandService _commands = new CustomCommandService(new Settings()
            {
                DefaultPrefix = Global.Prefix
            });

            handlerService = new HandlerService(_client);
            _handler = new CommandHandler(_client, _commands, handlerService);

            await Task.Delay(-1);

        }

        private async Task Log(LogMessage msg)
        {
            if (msg.Message == null)
                return;
            if (!msg.Message.StartsWith("Received Dispatch"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[Svt: {msg.Severity} Src: {msg.Source} Ex: {msg.Exception}] - " + msg.Message);
            }
        }
    }
}
