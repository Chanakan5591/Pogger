using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Pogger.Events
{
    public class UserJoinEvent
    {
        public async Task OnUserJoin(SocketGuildUser arg)
        {
            await arg.SendMessageAsync("Welcome lol");
        }
    }
}