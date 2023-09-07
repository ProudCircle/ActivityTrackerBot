using YamlDotNet.Serialization;

namespace ProudCircleActivityBot; 

public class YamlConfLoader {
    private string _configPath = "settings.yaml";

    public YamlConf YamlConf { get; private set; }

    public void LoadConfig() {
        string confContent = File.ReadAllText(_configPath);
        var deserializer = new DeserializerBuilder().Build();
        YamlConf = deserializer.Deserialize<YamlConf>(confContent);
    }
}