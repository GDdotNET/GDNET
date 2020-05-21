using System;
using System.Collections.Generic;
using System.Net.Http;
using GDNET.Extensions;
using GDNET.Extensions.Attributes;
using GDNET.Extensions.Serialization;
using GDNET.Server.IO.Net;

namespace GDNET.Server
{
    /// <summary>
    /// A level from the servers, not to be confused with Data.Level from the "GDNET.Client" namespace.
    /// </summary>
    public class Level
    {
        /// <summary>
        /// The ID of the level.
        /// </summary>
        [GdProperty(1)]
        public int Id { get; set; }

        /// <summary>
        /// The name of the level.
        /// </summary>
        [GdProperty(2)]
        public string Name { get; set; }

        #region Static Methods
        /// <summary>
        /// Options for searching levels, to optimize parameters.
        /// </summary>
        public class LevelSearchOptions
        {
            /// <summary>
            /// The page to search
            /// </summary>
            public int Page { get; set; } = 0;

            /// <summary>
            /// A custom song ID for Newgrounds, cannot be used with <see cref="LevelSearchOptions.Song" />.
            /// </summary>
            public int CustomSongId { get; set; } = 0;

            /// <summary>
            /// Filter for unrated levels.
            /// </summary>
            public bool IsUnrated { get; set; } = false;

            /// <summary>
            /// Filter for two-player levels.
            /// </summary>
            public bool TwoPlayer { get; set; } = false;

            /// <summary>
            /// Filter for featured levels.
            /// </summary>
            public bool Featured { get; set; } = false;

            /// <summary>
            /// Filter for epic levels.
            /// </summary>
            public bool Epic { get; set; } = false;

            /// <summary>
            /// Filter for levels with coins.
            /// </summary>
            public bool HasCoins { get; set; } = false;

            /// <summary>
            /// Filter for levels that aren't copied.
            /// </summary>
            public bool Original { get; set; } = false;

            /// <summary>
            /// Filter for level difficultues.
            /// </summary>
            public Difficulty[] Difficulty { get; set; } = { };

            /// <summary>
            /// Filter for level lengths.
            /// </summary>
            public Length[] Length { get; set; } = { };

            /// <summary>
            /// Filter for levels that use official game songs, cannot be used with <see cref="LevelSearchOptions.CustomSongId" />.
            /// </summary>
            public GameSong Song { get; set; } = GameSong.None;

            /// <summary>
            /// The type of search to do.
            /// </summary>
            public SearchType SearchType { get; set; } = SearchType.Search;

            /// <summary>
            /// Whether to search by a specific demon type or not.
            /// </summary>
            public DemonType DemonType { get; set; } = DemonType.None;
        }

        /// <summary>
        /// Gets a single level.
        /// </summary>
        /// <param name="search">A search query</param>
        /// <param name="options">Level search options.</param>
        /// <returns>A single level.</returns>
        public static Level GetLevel(string search = "", LevelSearchOptions options = null) =>
            GetLevels(search, options)[0];

        /// <summary>
        /// Gets an array of levels.
        /// </summary>
        /// <param name="search">A search query</param>
        /// <param name="options">Level search options.</param>
        /// <returns>The array.</returns>
        public static Level[] GetLevels(string search = "", LevelSearchOptions options = null)
        {
            if (options == null)
                options = new LevelSearchOptions();

            var content = new Dictionary<string, string>
            {
                { "gameVersion", "21" },
                { "binaryVersion", "35" },

                { "type", ((int)options.SearchType).ToString() },
                { "str", search },

                { "len", options.Length.Length > 0 ? options.Length.FormatEnum(",") : "-" },
                { "diff", options.Difficulty.Length > 0 ? options.Difficulty.FormatEnum(",") : "-" },

                { "coins", Convert.ToInt32(options.HasCoins).ToString() },
                { "noStar", Convert.ToInt32(options.IsUnrated).ToString() },
                { "twoPlayer", Convert.ToInt32(options.TwoPlayer).ToString() },
                { "featured", Convert.ToInt32(options.Featured).ToString() },
                { "epic", Convert.ToInt32(options.Epic).ToString() },
                { "original", Convert.ToInt32(options.Original).ToString() },

                { "secret", "Wmfd2893gb7" }
            };

            // extraneous/optional params
            if (options.DemonType != DemonType.None)
            {
                content["diff"] = "-2";
                content.Add("demonFilter", ((int)options.DemonType).ToString());
            }

            if (options.CustomSongId > 0 && options.Song > 0)
                throw new ArgumentException("You may not use NewGrounds Song ID's and In-game Song enums at once.");

            if (options.CustomSongId > 0 || options.Song > 0)
            {
                if (options.CustomSongId > 0)
                    content.Add("customSong", options.CustomSongId.ToString());
                else if (options.Song > 0)
                    content.Add("song", ((int)options.Song).ToString());
            }

            var result = WebRequestClient.SendRequest(new WebRequest
            {
                Url = "http://boomlings.com/database/getGJLevels21.php",
                Content = new FormUrlEncodedContent(content)
            });

            var levels = RobtopAnalyzer.DeserializeObjectList<Level>(result.Split("#")[0]);

            return levels.ToArray();
        }
        #endregion
    }

    #region Filter Enums
    /// <summary>
    /// An enum of official in-game songs.
    /// </summary>
    public enum GameSong
    {
        None = -1,
        StereoMadness = 1
    }

    /// <summary>
    /// An enum of search types.
    /// </summary>
    public enum SearchType
    {
        Search = 0,
        Downloaded = 1,
        Likes = 2,
        Trends = 3,
        Recent = 4,
        Magic = 7,
        Award = 11,
        Followed = 12,
        Friends = 13
    }

    /// <summary>
    /// An enum of the in-game difficulties.
    /// </summary>
    public enum Difficulty
    {
        Auto = -3,
        Demon = -2,
        Na = -1,
        Easy = 1,
        Normal = 2,
        Hard = 3,
        Harder = 4,
        Insane = 5
    }

    /// <summary>
    /// An enum of the demon types.
    /// </summary>
    public enum DemonType
    {
        None = -1,
        Easy = 1,
        Medium = 2,
        Hard = 3,
        Insane = 4,
        Extreme = 5
    }

    /// <summary>
    /// An enum of the lengths of levels.
    /// </summary>
    public enum Length
    {
        None = -1,
        Tiny,
        Short,
        Medium,
        Long,
        ExtraLong
    }
    #endregion
}