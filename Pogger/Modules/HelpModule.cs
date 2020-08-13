using System.Threading.Tasks;
using Discord.Addons.CommandsExtension;
using Discord.Commands;

namespace Pogger.Modules
{
    public class HelpModule : ModuleBase
    {
        private readonly CommandService _commandService;
        public HelpModule(CommandService commandService)
        {
            _commandService = commandService;
        }

        [Command("help"), Summary("Shows help menu.")]
        public async Task Help([Remainder] string command = null)
        {
            var helpEmbed = _commandService.GetDefaultHelpEmbed(command, Global.Prefix);
            await Context.Channel.SendMessageAsync(embed: helpEmbed);
        }
    }
}