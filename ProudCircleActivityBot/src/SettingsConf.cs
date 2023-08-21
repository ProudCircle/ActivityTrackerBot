using System.Text;
using System.Text.Json.Nodes;
using Newtonsoft.Json;

namespace ProudCircleActivityBot; 

public class SettingsConf {
    public SettingsConfLoader Loader { get; set; }
    
    /// <summary>
    /// Discord Login OAuth Token
    /// </summary>
    [JsonProperty("token")] public string Token { get; private set; }
    
    /// <summary>
    /// Prefix for text commands
    /// </summary>
    [JsonProperty("prefix")] public string Prefix { get; set; }
    
    /// <summary>
    /// Hypixel API Key
    /// </summary>
    [JsonProperty("hypixel_api_key")] public string HypixelApKey { get; set; }
}

public class SettingsConfLoader {
    public string ConfigPath = "settings.conf";
    public SettingsConf SettingsConf { get; private set; }

    /// <summary>
    /// Loads the configuration asynchronously
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task LoadConfigAsync() {
        var json = string.Empty;
        using (var fileStream = File.OpenRead(ConfigPath))
        using (var streamReader = new StreamReader(fileStream, new UTF8Encoding(false)))
            json = await streamReader.ReadToEndAsync();
        SettingsConf = JsonConvert.DeserializeObject<SettingsConf>(json) ?? throw new InvalidOperationException("Error loading config (Async)");
        SettingsConf.Loader = this;
    }

    /// <summary>
    /// Loads the configuration synchronously
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public void LoadConfigSync() {
        CheckConfig();
        try {
            var json = string.Empty;
            using (var fileStream = File.OpenRead(ConfigPath))
            using (var streamReader = new StreamReader(fileStream, new UTF8Encoding(false)))
                json = streamReader.ReadToEnd();
            SettingsConf = JsonConvert.DeserializeObject<SettingsConf>(json) ??
                           throw new InvalidOperationException("Error loading config (Sync)");
            SettingsConf.Loader = this;
        }
        catch (JsonException e) {
            throw new IOException($"Invalid config file: {e}");
        }
    }

    public void CheckConfig() {
        if (!File.Exists(ConfigPath)) {
            GenerateDefaults();
        }
    }

    public void GenerateDefaults() {
        Dictionary<string, string> conf = new Dictionary<string, string>();
        conf.Add("token", "");
        conf.Add("prefix", "!!");
        conf.Add("hypixel_api_key", "");
        var confStr = JsonConvert.SerializeObject(conf, Formatting.Indented);
        File.WriteAllText(ConfigPath, confStr);
    }
}