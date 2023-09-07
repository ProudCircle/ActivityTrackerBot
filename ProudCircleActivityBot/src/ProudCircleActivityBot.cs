using System.Timers;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.EventArgs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace ProudCircleActivityBot;

public class ProudCircleActivityBot {
    public DiscordClient DiscordClient { get; private set; }
    public CommandsNextExtension CommandsExtension { get; private set; }
    public SlashCommandsExtension SlashCommandsExtension { get; private set; }
    public SettingsConf Conf { get; private set; }
    private VersionInfo _versionInfo = new VersionInfo();

    /// <summary>
    /// Starts the discord bot.
    /// Loads the configuration file and
    /// connects the bot to the interwebs
    /// </summary>
    public async Task RunBotAsync() {
        var SettingsConfLoader = new SettingsConfLoader();
        // TODO: Throw/Catch Exception on Config Load
        SettingsConfLoader.LoadConfigSync(); // Load Sync first time
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

        // Text Commands
        var textCommandConfig = new CommandsNextConfiguration {
            StringPrefixes = new string[] { Conf.Prefix },
            EnableMentionPrefix = true,
            EnableDms = false,
            DmHelp = false,
            EnableDefaultHelp = false,
            Services = new ServiceCollection().AddSingleton(Conf).BuildServiceProvider()
            // TODO: Create custom help
        };
        CommandsExtension = DiscordClient.UseCommandsNext(textCommandConfig);
        CommandsExtension.RegisterCommands<PrefixCommands>();

        // Slash Commands
        var slashCommandConfig = new SlashCommandsConfiguration {
            Services = new ServiceCollection().AddSingleton(Conf).BuildServiceProvider()
        };
        SlashCommandsExtension = DiscordClient.UseSlashCommands(slashCommandConfig);
        SlashCommandsExtension.RegisterCommands<SlashCommands>();
        SlashCommandsExtension.SlashCommandErrored += OnSlashCommandErrored;

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
        discordClient.Logger.LogInformation(new EventId(900, "Startup"), $"Bot is ready ({_versionInfo.PrettyName})");
    }

    private async Task OnSlashCommandErrored(SlashCommandsExtension sender, SlashCommandErrorEventArgs args) {
        if (args.Exception is not SlashExecutionChecksFailedException) {
            DiscordClient.Logger.LogError(new EventId(804, "SlashCommandException"), args.Exception,
                $"Message: {args.Exception.Message}\nStack Trace: {args.Exception.StackTrace}");
            await args.Context.CreateResponseAsync(new ResponseEmbed().EmbedBuilder.WithTitle(":x: Oh no!")
                .WithDescription(
                    "It looks like there's been an error!\nPlease please contact my developer to report the issue!")
                .WithColor(new DiscordColor("#ff1010"))
                .Build());
            return;
        }

        await args.Context.CreateResponseAsync(new ResponseEmbed()
            .EmbedBuilder.WithColor(new DiscordColor("#FF0000"))
            .WithDescription(":x: You probably don't have permission for this command.\n" +
                             "If you think this is a bug please report it to my developer!").Build());
        DiscordClient.Logger.LogError(new EventId(800),
            $"a SlashCommandError occured: {args.Exception.Message} | {args.Exception.StackTrace}");
    }
}