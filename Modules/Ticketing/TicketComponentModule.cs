using NetCord;
using NetCord.Rest;
using NetCord.Services.ComponentInteractions;

namespace StellarSupport.Discord.Commands;

public class TicketComponentModule : ComponentInteractionModule<StringMenuInteractionContext>
{
    private const Permissions TicketAccess = Permissions.ViewChannel | Permissions.SendMessages | Permissions.ReadMessageHistory;

    [ComponentInteraction("ticket_category_select")]
    public async Task SelectCategoryAsync()
    {
        var categoryValue = Context.SelectedValues[0];
        var categoryChannelId = TicketConfig.GetCategory(categoryValue);

        var guild = Context.Guild;
        var user = Context.User;

        var overwrites = new List<PermissionOverwriteProperties>
        {
            new PermissionOverwriteProperties(guild!.Id, PermissionOverwriteType.Role)
                .WithDenied(Permissions.ViewChannel),
            new PermissionOverwriteProperties(user.Id, PermissionOverwriteType.User)
                .WithAllowed(TicketAccess),
        };

        var channelProperties = new GuildChannelProperties($"ticket-{user.Username}", ChannelType.TextGuildChannel)
            .WithTopic($"{user.Username} - {user.Id}")
            .WithPermissionOverwrites(overwrites)
            .WithParentId(categoryChannelId);

        var channel = await Context.Client.Rest.CreateGuildChannelAsync(guild.Id, channelProperties);

        var buttons = new ActionRowProperties().AddComponents([
            new ButtonProperties("close", "Close", ButtonStyle.Danger),
            // new ButtonProperties("escalate", "Escalate", ButtonStyle.Secondary), im too lazy to do it
        ]);

        await RespondAsync(InteractionCallback.Message(new InteractionMessageProperties()
            .WithFlags(MessageFlags.Ephemeral)
            .WithContent($"<#{channel.Id}>")
        ));

        await Context.Client.Rest.SendMessageAsync(
            channel.Id,
            new MessageProperties()
                .WithContent($"<@{user.Id}> Please state your issue or question immediately so that our support team can assist you.")
                .AddComponents(buttons)
        );
    }
}
