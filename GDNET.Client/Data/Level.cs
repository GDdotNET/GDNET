using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GDNET.Client.Encryption;
using GDNET.Client.IO;
using Object = GDNET.Client.Objects.Object;

namespace GDNET.Client.Data
{
    /// <summary>
    /// A class form of level data, parsed to be readable.
    /// </summary>
    public class Level
    {
        /// <summary>
        /// A parsed list of level objects.
        /// </summary>
        public readonly List<Object> LevelObjects = new List<Object>();

        /// <summary>
        /// The level string.
        /// </summary>
        public readonly string LevelString;

        /// <summary>
        /// An inherited <see cref="LocalLevel" />.
        /// </summary>
        public LocalLevel LocalLevel;

        public Level(string encodedLevelString)
        {
            LevelString = DecompressLevel(encodedLevelString);
            ParseObjects();
        }

        public Level(LocalLevel level)
        {
            LocalLevel = level;
            LevelString = DecompressLevel(level.LevelString);

            ParseObjects();
        }

        #region Private Methods
        /*private void ParseColours()
        {
            
        }

        private void ParseSettings()
        {
            
        }*/

        private void ParseObjects()
        {
            var objs = LevelString.Split(';');
            objs = objs.Skip(1).SkipLast(1).ToArray();

            foreach (var obj in objs)
                LevelObjects.Add(Object.Parse(obj));
        }
        #endregion

        #region Static Methods
        /// <summary>
        /// A way to load a level.
        /// </summary>
        /// <param name="localLevel">Loads over a <see cref="LocalLevel" /> instance.</param>
        /// <returns>A parsed level.</returns>
        public static Level Load(LocalLevel localLevel)
        {
            var level = new Level(localLevel);

            return level;
        }

        /// <summary>
        /// A way to load a level.
        /// </summary>
        /// <param name="localLevel">A level string.</param>
        /// <returns>A parsed level.</returns>
        public static Level Load(string encodedLevelString)
        {
            var level = new Level(encodedLevelString);

            return level;
        }

        /// <summary>
        /// Decompresses a level's string.
        /// </summary>
        /// <param name="level">A local level.</param>
        /// <returns>The decompressed level string.</returns>
        public static string DecompressLevel(LocalLevel level)
        {
            if (!level.LevelString.StartsWith("H4sIAAAAAAAA"))
                throw new ArgumentException("The provided level string is invalid.");

            return Encoding.ASCII.GetString(GameManager.Decompress(Base64.DecodeToBytes(level.LevelString)));
        }

        /// <summary>
        /// Decompresses a level's string.
        /// </summary>
        /// <param name="level">A proper level string.</param>
        /// <returns>The decompressed level string.</returns>
        public static string DecompressLevel(string level)
        {
            if (!level.StartsWith("H4sIAAAAAAAA"))
                throw new ArgumentException("The provided level string is invalid.");

            return Encoding.ASCII.GetString(GameManager.Decompress(Base64.DecodeToBytes(level)));
        }
        #endregion
    }
}