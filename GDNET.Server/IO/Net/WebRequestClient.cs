using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GDNET.Extensions;
using GDNET.Extensions.Exceptions;

namespace GDNET.Server.IO.Net
{
    /// <summary>
    /// A web request client made to make requests to the Boomlings API easier.
    /// </summary>
    public class WebRequestClient
    {
        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Options to the web client.
        /// </summary>
        public WebRequestClientOptions Options = new WebRequestClientOptions();

        private readonly Queue<WebRequest> pendingRequests = new Queue<WebRequest>();

        /// <summary>
        /// Sends the HTTP request in the current queue, if any.
        /// </summary>
        /// <returns>A string, as the response of the page.</returns>
        /// <exception cref="GdWebException"></exception>
        public string SendRequest()
        {
            var request = pendingRequests.Dequeue();
            var setupRequest = new HttpRequestMessage(request.Method, request.Url);

            foreach (var kvp in request.Headers) setupRequest.Headers.Add(kvp.Key, kvp.Value);

            setupRequest.Content = request.Content;
            var result = "";

            Task.Run(async () =>
            {
                using (var response = await client.SendAsync(setupRequest))
                {
                    result = await response.Content.ReadAsStringAsync();

                    if (int.TryParse(result, out var errorCode) && Options.IgnoreGdExceptions == false)
                        throw new GdWebException(((GdErrorType)errorCode).GetDescription())
                            { ErrorType = (GdErrorType)errorCode };
                }
            }).Wait();

            return result;
        }

        /// <summary>
        /// Sends a direct HTTP request.
        /// </summary>
        /// <param name="request">The HTTP Request.</param>
        /// <param name="options">Some options for the WebRequestClient.</param>
        /// <returns>A string, as the response of the page.</returns>
        /// <exception cref="GdWebException">An exception from the Boomlings API, which can be disabled to just return the code itself.</exception>
        public static string SendRequest(WebRequest request, WebRequestClientOptions options = null)
        {
            if (options == null)
                options = new WebRequestClientOptions();

            var setupRequest = new HttpRequestMessage(request.Method, request.Url);

            foreach (var kvp in request.Headers) setupRequest.Headers.Add(kvp.Key, kvp.Value);

            setupRequest.Content = request.Content;
            var result = "";

            Task.Run(async () =>
            {
                using (var response = await client.SendAsync(setupRequest))
                {
                    result = await response.Content.ReadAsStringAsync();

                    if (int.TryParse(result, out var errorCode) && options.IgnoreGdExceptions == false)
                        throw new GdWebException(((GdErrorType)errorCode).GetDescription())
                            { ErrorType = (GdErrorType)errorCode };
                }
            }).Wait();

            return result;
        }

        /// <summary>
        /// Queues/Adds a web request to the client.
        /// </summary>
        /// <param name="webRequest">The request to enqueue</param>
        public void AddRequest(WebRequest webRequest)
        {
            pendingRequests.Enqueue(webRequest);
        }

        /// <summary>
        /// Web request client options.
        /// </summary>
        public class WebRequestClientOptions
        {
            public bool IgnoreGdExceptions { get; set; }
        }
    }
}