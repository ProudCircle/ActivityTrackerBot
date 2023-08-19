using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace ProudCircleActivityBot; 

public class PrefixCommands : BaseCommandModule {
    // TODO: Custom Help Command (auto generated)
    [Command("help")]
    [Description("Show available commands")]
    public async Task HelpCommand(CommandContext ctx) {
        var embedBuilder = new DiscordEmbedBuilder()
            .WithTitle("Activity Tracker Help")
            .WithColor(DiscordColor.Magenta)
            .WithDescription($"All commands require a prefix: `!!help`")
            .AddField("ping", "Tests if bot is responsive")
            .AddField("version", "Shows the current version of the discord bot")
            .AddField("source", "View the bot's source code")
            .AddField("guildid", "View the Proud Circle Hypixel Guild ID")
            // .WithAuthor("illyum")
            .WithTimestamp(DateTimeOffset.Now)
            .WithFooter("Activity Tracker | v0.0.1");
        DiscordEmbed helpEmbed = embedBuilder.Build();
        await ctx.RespondAsync(helpEmbed);
    }
    
    [Command("ping")]
    [Description("Tests if bot is responsive")]
    public async Task PingCommand(CommandContext ctx) {
        await ctx.RespondAsync("Pong!");
    }

    [Command("version")]
    [Description("Shows the current version of the discord bot")]
    public async Task VersionCommand(CommandContext ctx) {
        await ctx.RespondAsync($"I'm currently on version 0.0.1!");
    }

    [Command("source")]
    [Description("View the bot's source code")]
    public async Task SourceCommand(CommandContext ctx) {
        await ctx.RespondAsync("You can view my source here: https://github.com/ProudCircle/ActivityTrackerBot");
    }

    [Command("guildid")]
    [Description("View the Proud Circle Hypixel Guild ID")]
    public async Task GuildIdCommand(CommandContext ctx) {
        await ctx.RespondAsync("Proud Circle Guild ID: 6177d2d68ea8c9a202fc277a");
    }
}