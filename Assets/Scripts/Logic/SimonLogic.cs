using System;
using System.Collections;
using System.Collections.Generic;
using SimonSays.Leaderboard;
using SimonSays.Menu;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SimonSays.Logic
{
    public enum Turns
    {
        Player,
        Computer
    }
    
    // This class is responsible for the game logic only
    // so there will be clear separation between the game logic and the UI (SimonPresenter)
    public class SimonLogic : MonoBehaviour
    {
        [SerializeField] private Settings _settings;
        [SerializeField] private Leaderboard.Leaderboard _leaderboard;

        private int _score;
        private Turns _currentTurn;
        private string _playerName;
        private int _movesIndex = 0;
        private bool _gameIsOn = false;
        private TimerLogic _timerLogic;
        private List<int> _computerMoves;
        private bool _oldMovesWasShown = false;
        
        public event Action OnResetGame;
        public event Action<bool> EndGame;
        public event Action<Turns> SetTurn;
        public event Action<int> UpdateScore;
        public event Action MakeComputerMove;
        public event Action<float> UpdateTimer;
        public event Action ShowComputerOldMoves;

        public WaitForSeconds _waitHalfSecond = new WaitForSeconds(0.5f);
        
        public void Start()
        {
            UpdatePlayerName();
            InitTimer();
            _gameIsOn = true;
            SetCurrentTurn(Turns.Computer);
            _computerMoves = new List<int>();
            MakeComputerMove?.Invoke();
        }

        private void InitTimer()
        {
            _timerLogic = new TimerLogic(_settings.GetTime());
            _timerLogic.TimerEnd += () => InvokeEndGame(_score > 0);
            _timerLogic.TimerChanged += time => UpdateTimer?.Invoke(time);
        }

        private void Update()
        {
            _timerLogic.Update(Time.deltaTime);
        }
        
        public void OnButtonClick(int buttonId)
        {
            StartCoroutine(HandlePlayerMove(buttonId));
        }

        private IEnumerator HandlePlayerMove(int buttonId)
        {
            if (IsCorrectMove(buttonId))
            {
                _movesIndex += 1;
                if (IsLastMove())
                {
                    SetCurrentTurn(Turns.Computer);
                    AddScore();
                    UpdateScore?.Invoke(GetCurrentScore());
                    _movesIndex = 0;
                    if (_settings.IsRepeatEnabled())
                    {
                        yield return StartCoroutine(RepeatMoves());
                    }
                    MakeComputerMove?.Invoke();
                }
            }
            else
            {
                InvokeEndGame(false);
            }
        }

        private bool IsCorrectMove(int buttonId)
        {
            return GetComputerMoves()[_movesIndex] == buttonId;
        }

        private bool IsLastMove()
        {
            return GetComputerMoves().Count == _movesIndex;
        }

        // This method is responsible for the computer's move
        private IEnumerator RepeatMoves()
        {
            yield return _waitHalfSecond;
            ShowComputerOldMoves?.Invoke();
            while (!_oldMovesWasShown)
            {
                yield return null;
            }
        }

        public void SetOldMovesShowed(bool showed)
        {
            _oldMovesWasShown = showed;
        }

        public void OrderLeaderboard()
        {
            _leaderboard.OrderLeaderboard(new LeaderboardPlayerData(_playerName,
                GetCurrentScore()));
        }

        private void InvokeEndGame(bool isWin)
        {
            EndGame?.Invoke(isWin);
            ResetGame();
        }

        private void ResetGame()
        {
            OnResetGame?.Invoke();
            _timerLogic.StopTimer();
            _gameIsOn = false;
            _score = 0;
            _computerMoves.Clear();
            _movesIndex = 0;
        }

        private void UpdatePlayerName()
        {
            _playerName = _settings.GetPlayerName();
        }

        public int GetButtonsAmount()
        {
            return _settings.GetButtonsAmount();
        }

        public void AddComputerMove(int id)
        {
            _computerMoves.Add(id);
        }

        private void AddScore()
        {
            _score += _settings.GetPoints();
        }

        public int GetCurrentScore()
        {
            return _score;
        }

        public List<int> GetComputerMoves()
        {
            return _computerMoves;
        }

        public void SetCurrentTurn(Turns turn)
        {
            _currentTurn = turn;
            SetTurn?.Invoke(_currentTurn);
        }

        public List<LeaderboardPlayerData> GetLeaderboardPlayers()
        {
            return _leaderboard.GetPlayers();
        }

        public bool IsGameOn()
        {
            return _gameIsOn;
        }

        public Turns GetCurrentTurn()
        {
            return _currentTurn;
        }

        public void SetBgMusicVolume(float volume)
        {
            _settings.SetBgMusicVolume(volume);
        }

        public int GetRandomButton()
        {
            return Random.Range(0, _settings.GetButtonsAmount());
        }
    }
}