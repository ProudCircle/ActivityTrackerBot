using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

namespace ProudCircleActivityBot; 

public class SlashCommands : ApplicationCommandModule {
    public SettingsConf Conf { private get; set; }
    
    
    [SlashCommand("status", "Shows some statistics for the bot")]
    public async Task StatusSlashCommand(InteractionContext ctx) {
        var responseEmbed = new ResponseEmbed();
        responseEmbed.EmbedBuilder.WithTitle("Status");
        responseEmbed.EmbedBuilder.WithDescription("I am alive!");
        await ctx.CreateResponseAsync(responseEmbed.EmbedBuilder.Build());
    }

    [SlashCommand("admin", "Test admin privileges")]
    [SlashRequirePermissions(Permissions.Administrator)]
    public async Task AdminSlashCommand(InteractionContext ctx) {
        var responseEmbed = new ResponseEmbed();
        responseEmbed.EmbedBuilder.WithTitle("Success!");
        responseEmbed.EmbedBuilder.WithDescription("Seems like you have permissions to run this command!");
        await ctx.CreateResponseAsync(responseEmbed.EmbedBuilder.Build());
    }
    
    [SlashCommand("testkey", "Tests the Hypixel API Key")]
    [SlashRequirePermissions(Permissions.Administrator)]
    public async Task TestKeySlashCommand(InteractionContext ctx) {
        var responseEmbed = new ResponseEmbed()
            .EmbedBuilder.WithTitle($"Key: '{Conf.HypixelApKey.Length-4 * '*'}{Conf.HypixelApKey[-4]}'");
        await ctx.CreateResponseAsync(responseEmbed.Build(), true);
    }
}