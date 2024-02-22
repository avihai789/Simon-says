using System.Linq;
using UnityEngine;

namespace SimonSays.Menu
{
    // Class that represents the settings of the game and acts as the api for the settings
    [CreateAssetMenu]
    public class Settings : ScriptableObject
    {
        public Config.Config Config { private get; set; }
        public int SelectedDifficultyLevel { get; set; }
        private string PlayerName { get; set; }

        private AudioSource _audioSource;

        public int GetButtonsAmount()
        {
            return Config.levelsData[SelectedDifficultyLevel].buttons;
        }

        public int GetPoints()
        {
            return Config.levelsData[SelectedDifficultyLevel].points;
        }

        public int GetTime()
        {
            return Config.levelsData[SelectedDifficultyLevel].time;
        }

        public bool IsRepeatEnabled()
        {
            return Config.levelsData[SelectedDifficultyLevel].isRepeat;
        }

        public string[] GetLevelNames()
        {
            return Config.levelsData.Select(level => level.levelName).ToArray();
        }

        public void SetName(string playerName)
        {
            PlayerName = playerName;
        }

        public string GetPlayerName()
        {
            return PlayerName;
        }

        public void SetAudioSource(AudioSource audioSource)
        {
            _audioSource = audioSource;
        }

        public void SetBgMusicVolume(float volume)
        {
            _audioSource.volume = volume;
        }

        public void ToggleSound()
        {
            _audioSource.mute = !_audioSource.mute;
        }
    }
}