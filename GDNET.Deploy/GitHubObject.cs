using Newtonsoft.Json;

namespace GDNET.Deploy
{
    public class GitHubObject
    {
        [JsonProperty(@"id")]
        public int Id;

        [JsonProperty(@"name")]
        public string Name;
    }
}