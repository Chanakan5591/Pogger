using BetterCommandService;
using Discord.WebSocket;
using System.Threading.Tasks;
using Discord.Commands;
using Discord;
using Pogger;

public class CommandHandler
{
    private DiscordSocketClient _client = Global.Client;
    private SocketCommandContext Context;
    private CustomCommandService _service;
    private HandlerService handlerService;

    public CommandHandler(DiscordSocketClient client, CustomCommandService service, HandlerService s)
    {
        _client = client;

        _client.SetGameAsync(Global.Status, null, ActivityType.Playing);

        _client.SetStatusAsync(UserStatus.Online);

        _service = service;

        handlerService = s;

    }
}