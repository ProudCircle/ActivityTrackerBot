using ProudCircleActivityBot;

public class YamlConf {
    public YamlConfLoader Loader { get; set; }
    
    public string token { set; get; }
    public string guid { get; set; } 
    public string apikey { get; set; }
    public string prefix { set; get; }
    public DatabaseConfig database { get; set; }
    public MySql mysql { get; set; }
    public Sqlite sqlite { get; set; }
}

public class DatabaseConfig {
    public string type { get; set; }
}

public class MySql {
    public string address { get; set; }
    public int port { get; set; }
    public string username { get; set; }
    public string password { get; set; }
    public string auth { get; set; }
}

public class Sqlite {
    public string filename { get; set; }
}