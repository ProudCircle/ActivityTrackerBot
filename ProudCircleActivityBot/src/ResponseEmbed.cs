using DSharpPlus.Entities;

namespace ProudCircleActivityBot; 

public class ResponseEmbed {
    public DiscordEmbedBuilder EmbedBuilder { get; set; }
    private readonly VersionInfo _versionInfo = new VersionInfo();
    
    public ResponseEmbed() {
        EmbedBuilder = new DiscordEmbedBuilder();
        // EmbedBuilder.WithAuthor("illyum");
        EmbedBuilder.WithColor(new DiscordColor("#0ec7c7"));
        EmbedBuilder.WithTimestamp(DateTimeOffset.Now);
        EmbedBuilder.WithFooter($"Activity Tracker | {_versionInfo.PrettyName}");
    }
}
