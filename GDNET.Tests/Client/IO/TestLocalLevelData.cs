using System;
using System.IO;
using System.Linq;
using GDNET.Client.Data;
using GDNET.Client.IO.Saves;
using GDNET.Client.Objects.Special;
using NUnit.Framework;

namespace GDNET.Tests.Client.IO
{
    public class TestLocalLevelManager
    {
        private readonly LocalLevelManager llm = new LocalLevelManager(
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GeometryDash") +
            Path.DirectorySeparatorChar + "CCLocalLevels.dat");

        [Test]
        public void TestDecoding()
        {
            llm.Parse();

            var level = llm.Levels[0];

            Assert.AreEqual("GDNET Big Brain", level.Name, "Not equal");
        }

        [Test]
        public void TestParseLevelString()
        {
            llm.Parse();

            var level = llm.Levels[0];
            var parsedLevel = Level.Load(level);

            var solidObjects = parsedLevel.LevelObjects.Where(o => o.Id == 1).Select(o => o);
            var textObject = parsedLevel.LevelObjects.FirstOrDefault(o => o.Id == 914) as TextObject;

            Assert.AreEqual(parsedLevel.LevelObjects.Count, 9);
            Assert.AreEqual(solidObjects.ToArray().Length, 9);
            Assert.AreEqual(textObject?.Text, "boring af level");
        }
    }
}