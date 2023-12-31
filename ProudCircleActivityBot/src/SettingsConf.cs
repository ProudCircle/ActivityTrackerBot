﻿using System.Text;
using Newtonsoft.Json;

namespace ProudCircleActivityBot; 

public class SettingsConf {
    public SettingsConfLoader Loader { get; set; }
    [JsonProperty("token")]
    public string Token { get; private set; }

    [JsonProperty("prefix")]
    public string Prefix { get; set; }
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
        var json = string.Empty;
        using (var fileStream = File.OpenRead(ConfigPath))
        using (var streamReader = new StreamReader(fileStream, new UTF8Encoding(false)))
            json = streamReader.ReadToEnd();
        SettingsConf = JsonConvert.DeserializeObject<SettingsConf>(json) ?? throw new InvalidOperationException("Error loading config (Sync)");
        SettingsConf.Loader = this;
    }
}