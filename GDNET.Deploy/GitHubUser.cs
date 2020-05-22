using Newtonsoft.Json;

namespace GDNET.Deploy
{
    public class GitHubUser
    {
        [JsonProperty(@"login")]
        public string Name;

        [JsonProperty(@"html_url")] 
        public string Link;
    }
}