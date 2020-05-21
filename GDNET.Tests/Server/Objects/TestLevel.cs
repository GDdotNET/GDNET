using GDNET.Server;
using NUnit.Framework;

namespace GDNET.Tests.Server.Objects
{
    public class TestLevel
    {
        [Test]
        public void TestGetLevels()
        {
            var levels = Level.GetLevels("umu");

            Assert.AreEqual("umulig", levels[0].Name, "Not the same!");
        }
    }
}