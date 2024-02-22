using System.IO;
using System.Xml.Serialization;

namespace SimonSays.Config
{
    public class XmlConfigLoad : IConfigLoad
    {
        public Config LoadConfig()
        {
            var xmlToString = File.ReadAllText("Assets/Configs/config.xml");
            var serializer = new XmlSerializer(typeof(Config));

            return (Config)serializer.Deserialize(new StringReader(xmlToString));
        }
    }
}