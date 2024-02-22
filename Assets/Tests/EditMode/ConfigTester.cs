using NUnit.Framework;
using SimonSays.Config;
using Assert = NUnit.Framework.Assert;

namespace Tests.EditMode
{
    public class ConfigTester
    {
        [Test]
        public void ButtonsValuesTestJsonSimplePasses()
        {
            var configLoader = new ConfigLoader();
            var loaded = configLoader.LoadConfigByType(new JsonConfigLoad());
            foreach (var t in loaded.levelsData)
            {
                Assert.IsTrue(t.buttons is >= 2 and <= 6);
            }
        }

        [Test]
        public void ButtonsValuesTestXmlSimplePasses()
        {
            var configLoader = new ConfigLoader();
            var loaded = configLoader.LoadConfigByType(new XmlConfigLoad());
            foreach (var t in loaded.levelsData)
            {
                Assert.IsTrue(t.buttons is >= 2 and <= 6);
            }
        }

        [Test]
        public void PointsValuesTestJsonSimplePasses()
        {
            var configLoader = new ConfigLoader();
            var loaded = configLoader.LoadConfigByType(new JsonConfigLoad());
            for (var i = 0; i < loaded.levelsData.Length; i++)
            {
                Assert.IsTrue(i + 1 == loaded.levelsData[i].points);
            }
        }

        [Test]
        public void PointsValuesTestXmlSimplePasses()
        {
            var configLoader = new ConfigLoader();
            var loaded = configLoader.LoadConfigByType(new XmlConfigLoad());
            for (var i = 0; i < loaded.levelsData.Length; i++)
            {
                Assert.IsTrue(i + 1 == loaded.levelsData[i].points);
            }
        }

        [Test]
        public void TimeValuesTestJsonSimplePasses()
        {
            var configLoader = new ConfigLoader();
            var loaded = configLoader.LoadConfigByType(new JsonConfigLoad());
            Assert.IsTrue(50 == loaded.levelsData[0].time);
            Assert.IsTrue(45 == loaded.levelsData[1].time);
            Assert.IsTrue(30 == loaded.levelsData[2].time);
        }

        [Test]
        public void TimeValuesTestXmlSimplePasses()
        {
            var configLoader = new ConfigLoader();
            var loaded = configLoader.LoadConfigByType(new XmlConfigLoad());
            Assert.IsTrue(50 == loaded.levelsData[0].time);
            Assert.IsTrue(45 == loaded.levelsData[1].time);
            Assert.IsTrue(30 == loaded.levelsData[2].time);
        }
    }
}