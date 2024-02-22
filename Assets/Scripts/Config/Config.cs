using System;
using System.Xml.Serialization;

namespace SimonSays.Config
{
    [Serializable]
    [XmlRoot("config")]
    public class Config
    {
        [XmlArray("levelsData")] [XmlArrayItem("levelData")]
        public LevelData[] levelsData;
        
        [Serializable]
        public class LevelData
        {
            [XmlAttribute] public string levelName;
            [XmlAttribute] public int buttons;
            [XmlAttribute] public int points;
            [XmlAttribute] public int time;
            [XmlAttribute] public bool isRepeat;
            [XmlAttribute] public float speed;
        }
    }
}