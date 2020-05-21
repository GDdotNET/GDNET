using System.Collections.Generic;
using System.Net.Http;
using NetWebRequest = System.Net.WebRequest;

namespace GDNET.Server.IO.Net
{
    /// <summary>
    /// A web request built to work with the GD servers.
    /// </summary>
    public class WebRequest
    {
        /// <summary>
        /// The headers of the request.
        /// </summary>
        public readonly Dictionary<string, string> Headers = new Dictionary<string, string>();

        /// <summary>
        /// The content of the request.
        /// </summary>
        public HttpContent Content { get; set; }

        /// <summary>
        /// The URL of the request.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// The HTTP method of the request.
        /// </summary>
        public HttpMethod Method { get; set; } = HttpMethod.Post;
    }
}