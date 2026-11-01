using NetCord;
using NetCord.Gateway;
using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;
using NetCord.Hosting.Services.ComponentInteractions;
using NetCord.Services.ApplicationCommands;
using NetCord.Services.ComponentInteractions;
using StellarSupport.Discord;

class Launch
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args).UseDiscordGateway(options => {
            options.Token = TicketConfig.BotToken;
            options.Presence = new PresenceProperties(UserStatusType.Online);
        })
        .UseApplicationCommands<SlashCommandInteraction, SlashCommandContext>()
        .UseComponentInteractions<ButtonInteraction, ButtonInteractionContext>()
        .UseComponentInteractions<StringMenuInteraction, StringMenuInteractionContext>();

        var host = builder.Build();

        host.AddModules(typeof(Launch).Assembly);

        var client = host.Services.GetRequiredService<GatewayClient>();

        client.Ready += async _ =>
        {
            while (true)
            {
                var guild = await client.Rest.GetGuildAsync(TicketConfig.GuildId);
                var channels = await guild.GetChannelsAsync();
                var ticketCount = channels.OfType<TextGuildChannel>().Where(x => x.Name.StartsWith("ticket-", StringComparison.OrdinalIgnoreCase)).Count();

                await client.UpdatePresenceAsync(new PresenceProperties(UserStatusType.Online)
                {
                    Activities = [ 
                        new UserActivityProperties($"Watching {ticketCount} tickets.", UserActivityType.Watching)
                    ]
                });

                await Task.Delay(30000);
            }
        };

        await host.RunAsync();
    }
}
