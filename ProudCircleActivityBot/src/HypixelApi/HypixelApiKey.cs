using System.Text.RegularExpressions;

namespace ProudCircleActivityBot.HypixelApi; 

public struct HypixelApiKey {
    public string ApiKey;
    private readonly Regex KeyPattern = new Regex(@"^[0-9a-fA-F]{32}$");
    
    public HypixelApiKey(string Key) {
        ApiKey = Key;
    }
    
    public HypixelApiKey(string key, bool TestKeyPattern = false) : this() {
        if (!TestKeyPattern) {
            ApiKey = key;
            return;
        }

        if (MatchesKeyPattern(key)) {
            ApiKey = key;
            return;
        }

        throw new ArgumentException("Invalid Key Pattern", paramName: key);
    }

    public bool MatchesKeyPattern(string key) {
        if (string.IsNullOrEmpty(key)) {
            throw new ArgumentNullException(paramName: key);
        }

        return KeyPattern.IsMatch(key);
    }
}