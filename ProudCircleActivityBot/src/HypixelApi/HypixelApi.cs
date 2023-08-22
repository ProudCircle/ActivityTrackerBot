using Newtonsoft.Json;
using ProudCircleActivityBot.HypixelApi.Replies;

namespace ProudCircleActivityBot.HypixelApi;

public class HypixelApi {
    public string BaseUrl = "https://api.hypixel.net/";
    public HypixelApiKey Key { get; private set; }
    private HttpClient HttpClient;

    public HypixelApi(HypixelApiKey key) {
        Key = key;
    }

    public async Task<PlayerReply> GetPlayerByUuidAsync(Guid guid) {
        try {
            string apiUrl = $"{BaseUrl}player?uuid={guid}";
            HttpClient.DefaultRequestHeaders.Add("API-Key", Key.ToString());
            HttpResponseMessage response = await HttpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode) {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                PlayerReply playerData = JsonConvert.DeserializeObject<PlayerReply>(jsonResponse);
                return playerData;
            }
            else {
                throw new HttpRequestException($"HTTP request failed with status code {response.StatusCode}");
            }
        }
        catch (Exception ex) {
            throw ex;
        }
    }
}