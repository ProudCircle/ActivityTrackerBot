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
        string maskedApiKey = string.Empty;
        if (Conf.HypixelApKey.Length < 4) {
            maskedApiKey = Conf.HypixelApKey;
        }
        else {
            int maskLength = Conf.HypixelApKey.Length - 4;
            for (int i = 0; i < maskLength; i++) {
                if (Conf.HypixelApKey[i] != '-') {
                    maskedApiKey = maskedApiKey + "#";
                }
                else {
                    maskedApiKey = maskedApiKey + "-";
                }
            }

            maskedApiKey = maskedApiKey + Conf.HypixelApKey.Substring(Conf.HypixelApKey.Length - 4);
        }

        var responseEmbed = new ResponseEmbed()
            .EmbedBuilder.WithTitle($"Key: '{maskedApiKey}'");
        await ctx.CreateResponseAsync(responseEmbed.Build(), true);
    }
}