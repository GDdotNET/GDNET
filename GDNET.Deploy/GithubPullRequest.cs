using System;
using Newtonsoft.Json;

namespace GDNET.Deploy
{
    public class GithubMinifiedPullRequest
    {
        [JsonProperty(@"merged_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime MergedAt;

        [JsonProperty(@"title")]
        public string Title;

        [JsonProperty(@"user")]
        public GithubUser User;

        [JsonProperty(@"number")]
        public int Number;
    }

    public class GithubPullRequest : GithubMinifiedPullRequest
    {
        [JsonProperty(@"merged")]
        public bool Merged;
    }
}