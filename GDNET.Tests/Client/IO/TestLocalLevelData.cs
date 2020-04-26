using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using GDNET.Client.Data;
using GDNET.Client.Encryption;
using GDNET.Client.IO.Saves;
using GDNET.Client.Objects;
using GDNET.Client.Objects.Special;
using NUnit.Framework;

namespace GDNET.Tests.Client.IO
{
    public class TestLocalLevelManager
    {
        private LocalLevelManager _llm = new LocalLevelManager(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GeometryDash") + Path.DirectorySeparatorChar + "CCLocalLevels.dat");

        [Test]
        public void TestDecoding()
        {
            _llm.Parse();

            LocalLevel level = _llm.Levels[0];

            Assert.AreEqual("aba", level.Name, "Not equal");
        }

        [Test]
        public void TestParseLevelString()
        {
            _llm.Parse();

            LocalLevel level = _llm.Levels[0];
            Level parsedLevel = Level.Load(level);
            
            var solidObjects = parsedLevel.LevelObjects.Where(o => o.Id == 1).Select(o => o);
            var textObject = parsedLevel.LevelObjects.FirstOrDefault(o => o.Id == 914) as TextObject;

            Assert.AreEqual(parsedLevel.LevelObjects.Count, 9);
            Assert.AreEqual(solidObjects.ToArray().Length, 6);
            Assert.AreEqual(textObject?.Text, "boring af level");
        }
    }
}
