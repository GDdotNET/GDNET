using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using GDNET.Client.Encryption;
using GDNET.Client.IO;
using NUnit.Framework;
using static System.Environment;

namespace GDNET.Tests.Client.Encryption
{
    public class TestEncryption
    {
        private const int key = 100;

        [Test]
        public void TestXorDecryption()
        {
            const string original = "If this text is readable then the test has succeeded!";

            var xor = Xor.Cipher(original, key);
            var cipher = Xor.Cipher(xor, key);

            Console.WriteLine(xor);
            Assert.AreEqual(original, cipher);
        }
        
        [Test]
        public void TestRobBase64Decryption()
        {
            var b64 = Base64.Encode("If this text is readable then the test has succeeded!");
            var encoded = Xor.Cipher(b64, key);
            
            Assert.AreEqual("If this text is readable then the test has succeeded!", RobBase64.Decode(encoded, key), "No u");
        }

        [Test]
        public void TestDecryptGameSave()
        {
            var path = Path.Combine(GetFolderPath(SpecialFolder.LocalApplicationData), "GeometryDash");
            var data = File.ReadAllText(path + Path.DirectorySeparatorChar + "CCLocalLevels.dat");

            var xored = Xor.Cipher(data, 11);
            var replaced = xored.Replace('-', '+').Replace('_', '/').Replace("\0", string.Empty);
            var gzipb64 = GameManager.Decompress(Base64.DecodeToBytes(replaced));

            Debug.WriteLine(Encoding.ASCII.GetString(gzipb64));

            File.WriteAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "test.xml"), gzipb64);
        }
        
    }
}
