using System.Collections.Generic;
using System.Net.Http;
using GDNET.Extensions.Serialization;
using GDNET.Server.IO.Net;

namespace GDNET.Server
{
    /// <summary>
    /// Similar to an account, but uses a login rather than search.
    /// </summary>
    public class UserAccount : User
    {
        /// <summary>
        /// The password of the account.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// A method to login to an account, creating a <see cref="UserAccount" /> object.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password</param>
        /// <returns>A user account, if successful.</returns>
        public static UserAccount Login(string username, string password)
        {
            var response = WebRequestClient.SendRequest(new WebRequest
            {
                Url = @"http://boomlings.com/database/accounts/loginGJAccount.php",
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "userName", username },
                    { "password", password },
                    { "secret", "Wmfv3899gc9" },
                    { "udid", "GDNET" }
                }),
                Method = HttpMethod.Post
            });

            return RobtopAnalyzer.DeserializeObject<UserAccount>(GetString(int.Parse(response.Split(',')[0])));
        }
    }
}