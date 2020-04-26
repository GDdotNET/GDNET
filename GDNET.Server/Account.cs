using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GDNET.Extensions.Attributes;
using GDNET.Extensions.Serialization;
using GDNET.Server.IO.Net;

namespace GDNET.Server
{
    /// <summary>
    /// An account from the servers.
    /// </summary>
    public class Account
    {
        /// <summary>
        /// The username of the account.
        /// </summary>
        [GdProperty(1)]
        public string Username { get; set; }

        /// <summary>
        /// The user ID of the account. Not to be confused with <see cref="AccountID"/>.
        /// </summary>
        [GdProperty(2)]
        public int UserID { get; set; }

        /// <summary>
        /// The account ID belonging to the account. Not to be confused with <see cref="UserID"/>
        /// </summary>
        [GdProperty(16)]
        public int AccountID { get; set; }

        /// <summary>
        /// The amount of secret coins the account has. Not to be confused with <see cref="UserCoins"/>
        /// </summary>
        [GdProperty(13)]
        public int SecretCoins { get; set; }

        /// <summary>
        /// The amount of user coins the account has. Not to be confused with <see cref="SecretCoins"/>
        /// </summary>
        [GdProperty(17)]
        public int UserCoins { get; set; }

        /// <summary>
        /// The moderator badge of the account, if any.
        /// </summary>
        [GdProperty(49)]
        public ModeratorType Badge { get; set; }

        /// <summary>
        /// The current cube the account has.
        /// </summary>
        [GdProperty(21)]
        public int Cube { get; set; }

        /// <summary>
        /// The current ship the account has.
        /// </summary>
        [GdProperty(22)]
        public int Ship { get; set; }

        /// <summary>
        /// The current ball the account has.
        /// </summary>
        [GdProperty(23)]
        public int Ball { get; set; }

        /// <summary>
        /// The current ufo the account has.
        /// </summary>
        [GdProperty(24)]
        public int Ufo { get; set; }

        /// <summary>
        /// The current wave the account has.
        /// </summary>
        [GdProperty(25)]
        public int Wave { get; set; }

        /// <summary>
        /// The current robot the account has.
        /// </summary>
        [GdProperty(26)]
        public int Robot { get; set; }

        /// <summary>
        /// The current spider the account has.
        /// </summary>
        [GdProperty(43)]
        public int Spider { get; set; }

        /// <summary>
        /// Whether the user has glow or not.
        /// </summary>
        [GdProperty(28)]
        public bool Glow { get; set; }

        #region Static Methods
        /// <summary>
        /// Gets an account from the servers.
        /// </summary>
        /// <param name="userID">The account ID of the user.</param>
        /// <returns>An account.</returns>
        public static Account Get(int userID) =>
            RobtopAnalyzer.DeserializeObject<Account>(WebRequestClient.SendRequest(new WebRequest
            {
                Url = "http://boomlings.com/database/getGJUserInfo20.php",
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "targetAccountID", userID.ToString() },
                    { "secret", "Wmfd2893gb7" }
                })
            }));

        /// <summary>
        /// Gets an account from the servers.
        /// </summary>
        /// <param name="userID">The account ID of the user.</param>
        /// <returns>An account.</returns>
        public static string GetString(int userID) =>
            WebRequestClient.SendRequest(new WebRequest
            {
                Url = "http://boomlings.com/database/getGJUserInfo20.php",
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "targetAccountID", userID.ToString() },
                    { "secret", "Wmfd2893gb7" }
                })
            });

        #endregion
    }

    /// <summary>
    /// An enum denoting moderator types.
    /// </summary>
    public enum ModeratorType
    {
        None,
        Normal,
        Elder
    }
}
