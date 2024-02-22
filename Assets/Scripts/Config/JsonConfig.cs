using System.IO;
using UnityEngine;

namespace SimonSays.Config
{
    public class JsonConfigLoad : IConfigLoad
    {
        public Config LoadConfig()
        {
            var jsonToString = File.ReadAllText("Assets/Configs/config.json");
            return JsonUtility.FromJson<Config>(jsonToString);
        }
    }
}