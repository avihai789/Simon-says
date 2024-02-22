namespace SimonSays.Leaderboard
{
    // This class is responsible for a single player in the leaderboard (not ui)
    public class LeaderboardPlayerData
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public bool IsCurrentPlayer { get; set; }

        public LeaderboardPlayerData(string name, int score)
        {
            Name = name;
            Score = score;
            IsCurrentPlayer = true;
        }
    }
}