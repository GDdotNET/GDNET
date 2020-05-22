using System;
using Newtonsoft.Json;

namespace GDNET.Deploy
{
    public class GithubRelease
    {
        [JsonProperty(@"id")]
        public int Id;

        [JsonProperty(@"tag_name")]
        public string TagName;
        
        [JsonProperty(@"name")]
        public string Name;

        [JsonProperty(@"draft")]
        public bool Draft;

        [JsonProperty(@"prerelease")]
        public bool PreRelease;

        [JsonProperty(@"upload_url")]
        public string UploadUrl;

        [JsonProperty(@"published_at", NullValueHandling = NullValueHandling.Ignore)] 
        public DateTime PublishedAt;

        [JsonProperty(@"body")]
        public string Description;
    }
}