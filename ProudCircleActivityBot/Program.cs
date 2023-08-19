class Program {
    static void Main(string[] args) {
        var bot = new ProudCircleActivityBot.ProudCircleActivityBot();
        bot.RunBotAsync().GetAwaiter().GetResult();
        Console.Out.WriteLine("Bot is logging off");
    }
}