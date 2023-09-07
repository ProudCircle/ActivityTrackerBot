using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using Microsoft.Data.Sqlite;

namespace ProudCircleActivityBot;

public class SlashCommands : ApplicationCommandModule {
    public SettingsConf Conf { private get; set; }

    // TODO: Custom Help Slash Command (auto generated)
    [SlashCommand("help", "Shows available slash commands")]
    public async Task HelpSlashCommand(InteractionContext ctx) {
        var responseEmbed = new ResponseEmbed();
        responseEmbed.EmbedBuilder.WithTitle("Activity Tracker Slash Commands Help");
        responseEmbed.EmbedBuilder.AddField("/status", "Shows some statistics for the bot");
        responseEmbed.EmbedBuilder.AddField("/admin", "Test admin privileges");
        responseEmbed.EmbedBuilder.AddField("/testkey", "Shows current config key (Admin Only)");
        await ctx.CreateResponseAsync(responseEmbed.EmbedBuilder.Build());
    }


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

    [SlashCommand("sqlite", "Test sqlite stuff")]
    [SlashRequirePermissions(Permissions.Administrator)]
    public async Task SqliteTestCommand(InteractionContext ctx) {
        string connectionSource = "Data Source=test.db";

        // Open the SQLite connection asynchronously
        using (var connection = new SqliteConnection(connectionSource)) {
            Console.Out.WriteLine("Openning Connection");
            await connection.OpenAsync();

            // Create the table if it doesn't exist
            var createTableCommand =
                new SqliteCommand(
                    "CREATE TABLE IF NOT EXISTS MyTable (id INTEGER PRIMARY KEY AUTOINCREMENT, JsonPlayerData TEXT)",
                    connection);
            await createTableCommand.ExecuteNonQueryAsync();
        }

        Console.Out.WriteLine("Created Table!");
        string url = "https://api.hypixel.net/uuid=5328930ed41149cb90ad4e5c7b27dd86";
        string playerData = "{}";
        try {
            using (HttpClient client = new HttpClient()) {
                client.DefaultRequestHeaders.Add("API-Key", Conf.HypixelApKey);
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode) {
                    playerData = await response.Content.ReadAsStringAsync();
                }
            }
        }
        catch (Exception e) {
            Console.Out.WriteLine($"Threw Exception: {e}");
        }

        // Open the SQLite connection again
        using (var connection = new SqliteConnection(connectionSource)) {
            await connection.OpenAsync();

            // Insert player data into the table
            var insertCommand =
                new SqliteCommand("INSERT INTO MyTable (JsonPlayerData) VALUES (@jsonData)", connection);
            insertCommand.Parameters.AddWithValue("@jsonData", playerData);
            await insertCommand.ExecuteNonQueryAsync();
        }
    }
}