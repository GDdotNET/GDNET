using System;
using System.IO;
using System.IO.Compression;
using GDNET.Client.Encryption;

namespace GDNET.Client.IO
{
    /// <summary>
    /// A normal game save manager.
    /// </summary>
    public abstract class GameManager
    {
        /// <summary>
        /// The inner bytes parsed from the data file.
        /// </summary>
        public byte[] InnerBytes;

        protected GameManager()
        {
        }

        protected GameManager(string filePath)
        {
            FilePath = filePath;
        }

        /// <summary>
        /// The type of data supported.
        /// </summary>
        public abstract DataType DataType { get; }

        /// <summary>
        /// The file path of the data.
        /// </summary>
        public string FilePath { get; set; }

        public abstract void Parse();

        /// <summary>
        /// A method to decompress the gamesave file to readable data for GDNET.
        /// </summary>
        /// <returns>Bytes, or readable data.</returns>
        public byte[] Decompress()
        {
            MemoryStream memory;

            var data = Base64.DecodeToBytes(Xor.Cipher(File.ReadAllText(FilePath), 11)
                .Replace('-', '+').Replace('_', '/').Replace("\0", string.Empty));

            using (var gZipStream = new GZipStream(new MemoryStream(data), CompressionMode.Decompress))
            {
                var buffer = new byte[4096];
                int count;

                using (memory = new MemoryStream())
                {
                    while ((count = gZipStream.Read(buffer, 0, 4096)) > 0)
                        memory.Write(buffer, 0, count);
                }

                return InnerBytes = memory.ToArray();
            }
        }

        /// <summary>
        /// A method to decompress raw bytes.
        /// </summary>
        /// <returns>More bytes, or readable data.</returns>
        public static byte[] Decompress(byte[] data)
        {
            var lengthBuffer = new byte[4];
            Array.Copy(data, data.Length - 4, lengthBuffer, 0, 4);

            using (var gZipStream = new GZipStream(new MemoryStream(data), CompressionMode.Decompress))
            {
                var buffer = new byte[BitConverter.ToInt32(lengthBuffer, 0)];

                gZipStream.Read(buffer, 0, BitConverter.ToInt32(lengthBuffer, 0));

                return buffer;
            }
        }
    }

    /// <summary>
    /// An enum of acceptable data type by a game manager.
    /// </summary>
    public enum DataType
    {
        GameData,
        LevelData
    }
}