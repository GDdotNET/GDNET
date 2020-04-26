using System;
using static System.Text.Encoding;
using Convert = System.Convert;

namespace GDNET.Client.Encryption
{
    /// <summary>
    /// Base64 encoding class, for mild data "encryption".
    /// Also allows for storing a text in it for safekeeping to encode and decode it over again.
    /// </summary>
    public class Base64
    {
        private string text;
        private bool encoded;

        public Base64(string newText)
        {
            text = newText;

            try
            {
                Decode(newText);
                encoded = true;
            }
            catch
            {
                // not encoded.
            }
        }

        /// <summary>
        /// Encodes the stored string to base64.
        /// </summary>
        /// <returns>The encoded string.</returns>
        public string Encode()
        {
            if (!encoded)
                text = Convert.ToBase64String(UTF8.GetBytes(text));
            else
                throw new InvalidOperationException("Cannot encode an already encoded string");

            encoded = true;
            return text;
        }

        /// <summary>
        /// Decodes the stored string to normal text.
        /// </summary>
        /// <returns>The decoded string.</returns>
        public string Decode()
        {
            if (encoded)
                text = UTF8.GetString(Convert.FromBase64String(text));
            else
                throw new InvalidOperationException("Cannot decode an already decoded string");

            encoded = false;
            return text;
        }

        // static methods
        /// <summary>
        /// Static encoding, encodes a string to base64.
        /// </summary>
        /// <param name="text">The text to encode.</param>
        /// <returns>The encoded string.</returns>
        public static string Encode(string text) =>
            Convert.ToBase64String(UTF8.GetBytes(text));

        /// <summary>
        /// Static decoding, decodes a base64 string to a normal one.
        /// </summary>
        /// <param name="text">The base64 string to decode.</param>
        /// <returns>The decoded string.</returns>
        public static string Decode(string base64) =>
            UTF8.GetString(Convert.FromBase64String(base64.Replace('-', '+').Replace('_', '/')));

        public static string EncodeFromBytes(byte[] bytes) =>
            Convert.ToBase64String(bytes);

        public static byte[] DecodeToBytes(string base64) =>
            Convert.FromBase64String(base64.Replace('-', '+').Replace('_', '/'));
    }
}