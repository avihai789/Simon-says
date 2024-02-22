using System.Collections;
using System.Collections.Generic;
using SimonSays.Leaderboard;
using SimonSays.Logic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SimonSays.Presenter
{
    // This class is responsible for the game scene UI only
    public class SimonPresenter : MonoBehaviour
    {
        [SerializeField] private SimonButton _buttonPrefab;
        [SerializeField] private Transform _buttonsTopContainer;
        [SerializeField] private Transform _buttonsBottomContainer;
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private TextMeshProUGUI _gameOverScoreText;
        [SerializeField] private TextMeshProUGUI _gameOverText;
        [SerializeField] private TextMeshProUGUI _turnText;
        [SerializeField] private Transform _leaderboardContainer;
        [SerializeField] private LeaderboardPlayer _leaderboardPlayer;
        [SerializeField] private SimonLogic _simonLogic;

        private readonly List<SimonButton> _buttons = new List<SimonButton>();
        private readonly Color[] _colors = new Color[]
            { Color.red, Color.green, Color.blue, Color.yellow, Color.cyan, Color.magenta };

        private const string PLAYER_TURN_TEXT = "Your turn!";
        private const string COMPUTER_TURN_TEXT = "Computer turn!";
        private const string PLAYER_WIN_TEXT = "You win!";
        private const string GAME_OVER_TEXT = "Game over!";

        private void Start()
        {
            SubscribeToEvents();
            CreateButtons();
        }

        private void SubscribeToEvents()
        {
            _simonLogic.EndGame += EndGame;
            _simonLogic.SetTurn += SetTurnText;
            _simonLogic.OnResetGame += ResetGame;
            _simonLogic.UpdateTimer += SetTimerText;
            _simonLogic.UpdateScore += SetScoreText;
            _simonLogic.MakeComputerMove += MakeComputerMove;
            _simonLogic.ShowComputerOldMoves += ShowComputerOldMoves;
        }

        private void CreateButtons()
        {
            for (var i = 0; i < _simonLogic.GetButtonsAmount(); i++)
            {
                var button = CreateButton(i);
                button.Click += _simonLogic.OnButtonClick;
                AddButton(button);
            }
        }

        private void ResetButtons()
        {
            foreach (var button in _buttons)
            {
                button.Click -= _simonLogic.OnButtonClick;
            }

            _buttons.Clear();
        }

        private void DisableAllButtons()
        {
            foreach (var button in _buttons)
            {
                button.DisableButton();
            }
        }

        private void ReleaseAllButtons()
        {
            foreach (var button in _buttons)
            {
                button.EnableButton();
            }
        }

        private void AddButton(SimonButton button)
        {
            _buttons.Add(button);
        }

        private void MakeComputerMove()
        {
            StartCoroutine(MakeMove());
        }

        private IEnumerator MakeMove()
        {
            yield return _simonLogic._waitHalfSecond;
            PickNumberAndMove();
            yield return _simonLogic._waitHalfSecond;
            ChangeTurn();
        }

        private void PickNumberAndMove()
        {
            var index = _simonLogic.GetRandomButton();
            var randomButton = _buttons[index];
            StartCoroutine(randomButton.ComputerMove());
            _simonLogic.AddComputerMove(randomButton.GetButtonId());
        }

        private void ChangeTurn()
        {
            ReleaseAllButtons();
            _simonLogic.SetCurrentTurn(Turns.Player);
            SetTurnText(_simonLogic.GetCurrentTurn());
        }

        private void ShowComputerOldMoves()
        {
            StartCoroutine(ShowOldMoves());
        }

        private IEnumerator ShowOldMoves()
        {
            _simonLogic.SetOldMovesShowed(false);
            foreach (var move in _simonLogic.GetComputerMoves())
            {
                if (_simonLogic.IsGameOn())
                {
                    yield return _simonLogic._waitHalfSecond;
                    StartCoroutine(_buttons[move].ComputerMove());
                }
            }
            _simonLogic.SetOldMovesShowed(true);
        }

        private void ResetGame()
        {
            ResetButtons();
        }

        private void EndGame(bool isWin)
        {
            if (isWin)
            {
                _simonLogic.OrderLeaderboard();
                _leaderboardContainer.gameObject.SetActive(true);
                _scoreText.gameObject.SetActive(true);
            }
            _simonLogic.SetBgMusicVolume(0.8f);
            SetGameOverText(isWin ? PLAYER_WIN_TEXT : GAME_OVER_TEXT);
            SetGameOverScoreText(_simonLogic.GetCurrentScore());
            ShowGameOverPanel();
            SetLeaderboard(_simonLogic.GetLeaderboardPlayers());
        }

        private SimonButton CreateButton(int i)
        {
            var button = Instantiate(_buttonPrefab, i % 2 == 0 ? _buttonsTopContainer : _buttonsBottomContainer);
            button.SetButtonId(i, _colors[i]);
            return button;
        }

        private void SetTimerText(float time)
        {
            _timerText.text = Mathf.Ceil(time).ToString();
        }

        private void SetScoreText(int score)
        {
            _scoreText.text = score.ToString();
        }

        private void ShowGameOverPanel()
        {
            _gameOverPanel.SetActive(true);
        }

        private void SetGameOverScoreText(int score)
        {
            _gameOverScoreText.text = "Your score: " + score;
        }

        private void SetGameOverText(string text)
        {
            _gameOverText.text = text;
        }

        public void OnClickBackToMenu()
        {
            _simonLogic.SetBgMusicVolume(0.8f);
            ResetGame();
            SceneManager.UnloadSceneAsync("GameScene");
        }

        private void SetTurnText(Turns turn)
        {
            if (turn == Turns.Player)
            {
                _turnText.text = PLAYER_TURN_TEXT;
                ReleaseAllButtons();
            }
            else
            {
                _turnText.text = COMPUTER_TURN_TEXT;
                DisableAllButtons();
            }
        }

        private void SetLeaderboard(List<LeaderboardPlayerData> playerData)
        {
            var position = 1;
            foreach (var player in playerData)
            {
                var leaderboardPlayer = Instantiate(_leaderboardPlayer, _leaderboardContainer);
                leaderboardPlayer.SetPlayer(player.Score, player.Name, position, player.IsCurrentPlayer);
                position++;
            }
        }

        private void OnDestroy()
        {
            _simonLogic.EndGame -= EndGame;
            _simonLogic.SetTurn -= SetTurnText;
            _simonLogic.OnResetGame -= ResetGame;
            _simonLogic.UpdateScore -= SetScoreText;
            _simonLogic.UpdateTimer -= SetTimerText;
            _simonLogic.MakeComputerMove -= MakeComputerMove;
            _simonLogic.ShowComputerOldMoves -= ShowComputerOldMoves;
        }
    }
}