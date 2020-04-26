using System;
using System.Collections.Generic;
using System.Text;

namespace GDNET.Client.Encryption
{
    public static class Xor
    {
        public static string Cipher(string text, int key)
        {
            byte[] result = new byte[text.Length];
 
            for (int c = 0; c < text.Length; c++)
                result[c] = (byte)((uint)text[c] ^ key);

            return Encoding.UTF8.GetString(result);
        }
    }
}
