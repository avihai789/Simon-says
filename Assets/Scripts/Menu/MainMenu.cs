using System;
using System.Collections;
using SimonSays.Config;
using SimonSays.Logic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SimonSays.Menu
{
    // Class that handles the main menu logic and UI together
    // I didn't split the UI and logic because it's a small class and it's easier to maintain
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Settings _settings;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private GameObject _nameErrorrMessage;
        [SerializeField] private TMP_InputField _playerNameInput;
        [SerializeField] private DifficultyButton _difficultyButton;
        [SerializeField] private GameObject _difficultyButtonsContainer;
        [SerializeField] private GameObject _difficultyErrorrMessage;

        private SimonLogic _simonLogic;
        private readonly WaitForSeconds _waitTwoSeconds = new WaitForSeconds(2);

        private void Awake()
        {
            LoadConfig();
        }

        private void Start()
        {
            _settings.SetAudioSource(_audioSource);
            SetMainMenu();
        }

        // The JsonConfigLoad can be changed to XmlConfigLoad or another type of config loader in future
        private void LoadConfig()
        {
            var config = new ConfigLoader().LoadConfigByType(new JsonConfigLoad());
            if (config != null)
            {
                _settings.Config = config;
            }
            else
            {
                throw new Exception("Config not loaded");
            }
        }

        private void SetMainMenu()
        {
            SetDifficultyButtons();
        }

        private void SetDifficultyButtons()
        {
            for (var i = 0; i < _settings.GetLevelNames().Length; i++)
            {
                var button = Instantiate(_difficultyButton, _difficultyButtonsContainer.transform);
                button.SetDifficulty(i, _settings.GetLevelNames()[i]);
                button.Clicked += SetDifficulty;
            }

            _settings.SelectedDifficultyLevel = -1;
        }

        private void SetDifficulty(int difficulty)
        {
            _settings.SelectedDifficultyLevel = difficulty;
        }

        public void OnStartButtonClick()
        {
            if (ValidateStartGame())
            {
                SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Additive).completed += GameSceneLoaded;
            }
        }

        private void GameSceneLoaded(AsyncOperation asyncOperation)
        {
            _settings.SetBgMusicVolume(0.1f);
            _settings.SetName(_playerNameInput.text);
        }

        private bool ValidateStartGame()
        {
            return Validate(_settings.SelectedDifficultyLevel != -1, _difficultyErrorrMessage) &&
                   Validate(_playerNameInput.text.Length > string.Empty.Length, _nameErrorrMessage);
        }

        private bool Validate(bool condition, GameObject errorMessage)
        {
            if (condition) return true;
            StartCoroutine(ShowErrorMessage(errorMessage));

            return false;
        }

        private IEnumerator ShowErrorMessage(GameObject errorMessage)
        {
            errorMessage.SetActive(true);
            yield return _waitTwoSeconds;
            errorMessage.SetActive(false);
        }

        public void OnToggleSound()
        {
            _settings.ToggleSound();
        }
    }
}