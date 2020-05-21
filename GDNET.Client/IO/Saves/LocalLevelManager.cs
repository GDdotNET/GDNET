using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using GDNET.Client.Data;

namespace GDNET.Client.IO.Saves
{
    /// <summary>
    /// A manager for CCLocalLevels.dat, for created and saved levels.
    /// </summary>
    public class LocalLevelManager : GameManager
    {
        /// <summary>
        /// The data type accepted by the manager, being level data.
        /// </summary>
        public override DataType DataType => DataType.LevelData;

        /// <summary>
        /// The levels in the save data.
        /// </summary>
        public readonly List<LocalLevel> Levels = new List<LocalLevel>();

        public LocalLevelManager(string filePath)
        {
            FilePath = filePath;
        }

        /// <summary>
        /// Parses levels to the manager.
        /// </summary>
        public override void Parse()
        {
            if (InnerBytes == null)
                Decompress();

            var levels = Regex.Split(Encoding.ASCII.GetString(InnerBytes), @"(?:<k>)k_.(?:<\/k>)");

            for (var i = 1; i < levels.Length; i++)
            {
                var level = levels[i];

                if (level.EndsWith("</d><k>LLM_02</k><i>35</i></dict></plist>"))
                    level = level.Substring(0, level.Length - "</d><k>LLM_02</k><i>35</i></dict></plist>".Length);

                Levels.Add(LocalLevel.Load(level));
            }
        }

        /// <summary>
        /// Gets a <see cref="KeyValuePair{TKey,TValue}" /> in the XML readable save data.
        /// </summary>
        /// <param name="level">Level string to take the <see cref="KeyValuePair{TKey,TValue}" /> from.</param>
        /// <param name="key">The key to get.</param>
        /// <param name="type">Tye type to pass it to.</param>
        /// <returns></returns>
        public static KeyValuePair<string, object> GetPair(string level, string key, Type type)
        {
            var match = Regex.Match(level, @"<k>" + key + @"<\/k><(?:i|b|s|u)>([-_\w\d\s=]{0,})<\/(?:i|b|s|u)>");

            if (match.Groups.Count < 2)
                return new KeyValuePair<string, object>(key, Convert.ChangeType(null, type));

            return new KeyValuePair<string, object>(key, Convert.ChangeType(match.Groups[1].Value, type));
        }
    }
}