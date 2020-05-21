using System.Text;

namespace GDNET.Client.Encryption
{
    public static class Xor
    {
        public static string Cipher(string text, int key)
        {
            var result = new byte[text.Length];

            for (var c = 0; c < text.Length; c++)
                result[c] = (byte)((uint)text[c] ^ key);

            return Encoding.UTF8.GetString(result);
        }
    }
}