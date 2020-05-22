using System;
using Newtonsoft.Json;

namespace GDNET.Deploy
{
    public class GitHubMinifiedPullRequest
    {
        [JsonProperty(@"merged_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime MergedAt;

        [JsonProperty(@"title")]
        public string Title;

        [JsonProperty(@"user")]
        public GitHubUser User;

        [JsonProperty(@"number")]
        public int Number;
    }

    public class GitHubPullRequest : GitHubMinifiedPullRequest
    {
        [JsonProperty(@"merged")]
        public bool Merged;
    }
}