namespace GDNET.Client.Encryption
{
    public static class RobBase64
    {
        public static string Decode(string data, int key) => 
            Base64.Decode(Xor.Cipher(data, key));
    }
}
