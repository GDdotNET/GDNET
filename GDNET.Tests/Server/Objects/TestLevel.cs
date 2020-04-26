﻿using GDNET.Server;
using NUnit.Framework;

namespace GDNET.Tests.Server.Objects
{
    public class TestLevel
    {
        [Test]
        public static void TestGetLevels()
        {
            Level[] levels = Level.GetLevels("umu");

            Assert.AreEqual("umulig", levels[0].Name, "Not the same!");
        }
    }
}
