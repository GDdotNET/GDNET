using Newtonsoft.Json;

namespace GDNET.Deploy
{
    public class GithubObject
    {
        [JsonProperty(@"id")]
        public int Id;

        [JsonProperty(@"name")]
        public string Name;
    }
}