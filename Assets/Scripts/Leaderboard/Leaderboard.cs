using System.Collections.Generic;
using UnityEngine;

namespace SimonSays.Leaderboard
{
    // This class is responsible for the leaderboard scriptable object data
    [CreateAssetMenu]
    public class Leaderboard : ScriptableObject
    {
        private readonly List<LeaderboardPlayerData> _leaderboardPlayers = new List<LeaderboardPlayerData>();

        private void AddPlayer(LeaderboardPlayerData player)
        {
            _leaderboardPlayers.Add(player);
        }

        public List<LeaderboardPlayerData> GetPlayers()
        {
            return _leaderboardPlayers;
        }

        private void SotByScore()
        {
            _leaderboardPlayers.Sort((x, y) => y.Score.CompareTo(x.Score));
        }

        public void OrderLeaderboard(LeaderboardPlayerData newPlayer)
        {
            foreach (var player in _leaderboardPlayers)
            {
                player.IsCurrentPlayer = false;
            } 
            
            AddPlayer(newPlayer);
            SotByScore();
        }
    }
}