using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Logging;

namespace ProudCircleActivityBot;

public class ProudCircleActivityBot {
    public DiscordClient DiscordClient { get; private set; }
    public CommandsNextExtension CommandsExtension { get; private set; }
    public SettingsConf Conf { get; private set; }

    /// <summary>
    /// Starts the discord bot.
    /// Loads the configuration file and
    /// connects the bot to the interwebs
    /// </summary>
    public async Task RunBotAsync() {
        var SettingsConfLoader = new SettingsConfLoader();
        // TODO: Throw/Catch Exception on Config Load
        SettingsConfLoader.LoadConfigSync();  // Load Sync first time
        Conf = SettingsConfLoader.SettingsConf;

        var discordConfiguration = new DiscordConfiguration {
            AutoReconnect = true,
            Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents,
            LogTimestampFormat = "MMM dd yyyy - hh:mm:ss tt",
            MinimumLogLevel = LogLevel.Debug,
            Token = Conf.Token,
            TokenType = TokenType.Bot,
            LogUnknownEvents = false
            // TODO: Use custom bot logger factory
            // LoggerFactory = BotLoggerFactory
        };

        DiscordClient = new DiscordClient(discordConfiguration);
        RegisterEvents(DiscordClient);
        
        var textCommandConfig = new CommandsNextConfiguration {
            StringPrefixes = new string[] {Conf.Prefix},
            EnableMentionPrefix = true,
            EnableDms = false,
            DmHelp = false,
            EnableDefaultHelp = false,
            // TODO: Create custom help
        };
        CommandsExtension = DiscordClient.UseCommandsNext(textCommandConfig);
        CommandsExtension.RegisterCommands<PrefixCommands>();
        
        // Start Bot
        DiscordActivity activity = new DiscordActivity("every guild member!", ActivityType.Watching);
        await DiscordClient.ConnectAsync(activity);
        await Task.Delay(-1);
    }

    private void RegisterEvents(DiscordClient discordClient) {
        // On Ready
        discordClient.Ready += OnReadyEvent;
    }

    private async Task OnReadyEvent(DiscordClient discordClient, ReadyEventArgs eventArgs) {
        discordClient.Logger.LogInformation(new EventId(900, "Startup"), "Bot has logged in!");
    }
}