using NetCord;

namespace StellarSupport.Discord;

static class TicketConfig
{
    public const ulong GuildId = 0000000000000000000;
    public const string BotToken = "";
    public static readonly (string Value, string Label, string Description, ulong categoryId, EmojiProperties Emoji)[] Categories =
    [
        ("support", "Support Ticket", "I need help.", 1540811583368855632, EmojiProperties.Standard("❓")),
        ("appeal", "Ban Appeal", "Appeal your ban.", 1540812454362226748, EmojiProperties.Standard("🚫")),
        ("donation", "Donation Ticket", "Donate to Stellar.", 1540812503683178508, EmojiProperties.Standard("💸")),
    ];

    public static ulong GetCategory(string value)
    {
        foreach (var category in Categories)
        {
            if (category.Value == value)
                return category.categoryId;
        }

        return 0;
    }
}