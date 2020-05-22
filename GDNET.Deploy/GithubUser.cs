using Newtonsoft.Json;

namespace GDNET.Deploy
{
    public class GithubUser
    {
        [JsonProperty(@"login")]
        public string Name;

        [JsonProperty(@"html_url")] 
        public string Link;
    }
}