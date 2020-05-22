using Newtonsoft.Json;

namespace GDNET.Deploy
{
    public class DiscordWebhook
    {
        [JsonProperty("content")]
        public string Content;
    }
}