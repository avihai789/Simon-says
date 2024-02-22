using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SimonSays.Leaderboard
{
// This class is responsible for a single player in the leaderboard
    public class LeaderboardPlayer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _score;
        [SerializeField] private TextMeshProUGUI _position;
        [SerializeField] private Image _panel;

        // Set player data in the ui
        public void SetPlayer(int score, string name, int position, bool isMe)
        {
            _name.text = name;
            _score.text = score.ToString();
            _position.text = position.ToString();
            if (!isMe)
            {
                _panel.color = Color.white;
            }
        }
    }
}