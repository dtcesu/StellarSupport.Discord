using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace StellarSupport.Discord.Modules.Ticketing;

public class SetupCommandModule : ApplicationCommandModule<SlashCommandContext>
{
    [SlashCommand("setup", "Post the create a ticket panel", DefaultGuildPermissions = Permissions.ManageChannels)]
    public async Task CreateAsync()
    {
        var options = new List<StringMenuSelectOptionProperties>();

        foreach (var category in TicketConfig.Categories)
        {
            options.Add(new StringMenuSelectOptionProperties(category.Label, category.Value)
                .WithDescription(category.Description)
                .WithEmoji(category.Emoji)
            );
        }

        var menu = new StringMenuProperties("ticket_category_select", options).WithPlaceholder("Select a ticket category");

        var componentContainer = new ComponentContainerProperties().AddComponents([
            new TextDisplayProperties(@"## Create a Ticket
To create a ticket, please select the ticket category."), menu
        ]);

        await Context.Client.Rest.SendMessageAsync(Context.Channel.Id, new MessageProperties {
            Flags = MessageFlags.IsComponentsV2,
            Components = [componentContainer]
        });
    }
}
