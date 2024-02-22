using System;
using TMPro;
using UnityEngine;

namespace SimonSays.Menu
{
    // Class that represents a button that sets the difficulty level
    public class DifficultyButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _difficultyName;

        private int _difficulty;

        public event Action<int> Clicked;

        public void SetDifficulty(int difficulty, string difficultyName)
        {
            _difficulty = difficulty;
            _difficultyName.text = difficultyName;
        }

        public void OnClick()
        {
            Clicked?.Invoke(_difficulty);
        }
    }
}