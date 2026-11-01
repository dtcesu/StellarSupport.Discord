using NetCord;
using NetCord.Rest;
using NetCord.Services.ComponentInteractions;

namespace StellarSupport.Discord.Modules.Ticketing
{
    public class CloseTicketComponentModule : ComponentInteractionModule<ButtonInteractionContext>
    {
        [ComponentInteraction("close")]
        public async Task CloseAsync()
        {
            var actions = new ActionRowProperties().AddComponents(
            [
                new ButtonProperties("close-confirm", "Close", ButtonStyle.Danger),
                new ButtonProperties("close-cancel", "Cancel", ButtonStyle.Secondary),
            ]);

            await RespondAsync(InteractionCallback.Message(new InteractionMessageProperties
            {
                Content = "Are you sure you want to close this ticket?",
                Components = [actions],
            }));
        }

        [ComponentInteraction("close-confirm")]
        public async Task ConfirmCloseAsync()
        {
            // aaa
        }

        [ComponentInteraction("close-cancel")]
        public async Task CancelCloseAsync()
        {
            await Context.Client.Rest.DeleteMessageAsync(Context.Channel.Id, Context.Message.Id);
        }
    }
}