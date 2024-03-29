using CatMatch.Helpers;

namespace CatMatch.Services;

public interface IScoreService
{
    State<string> Score { get; }
    State<string> HighScore { get; }
    State<string> Streak { get; }
    State<string> HighStreak { get; }

    void IncrementScore();
    
    void ReserStreak();
}

public class ScoreService : IScoreService
{
    private State<string> _score;
    private State<string> _highScore;
    private State<string> _streak;
    private State<string> _highStreak;

    public ScoreService()
    {
        _score = new State<string>("0");
        _streak = new State<string>("0");
        
        var highScore = Preferences.Get("HighScore", 0);
        _highScore = new State<string>(highScore.ToString());
        
        var highStreak = Preferences.Get("HighStreak", 0);
        _highStreak = new State<string>(highStreak.ToString());
    }

    public State<string> Score => _score;
    public State<string> HighScore => _highScore;
    public State<string> Streak => _streak;
    public State<string> HighStreak => _highStreak;

    public void IncrementScore()
    {
        var currentScore = int.Parse(_score.CurrentValue);
        _score.SetValue((currentScore + 1).ToString());
        CheckHighScore();
        var currentStreak = int.Parse(_streak.CurrentValue);
        _streak.SetValue((currentStreak + 1).ToString());
        CheckHighStreak();
    }
    
    public void ReserStreak()
    {
        _streak.SetValue("0");
    }
    
    private void CheckHighScore()
    {
        var currentScore = int.Parse(_score.CurrentValue);
        var highScore = int.Parse(_highScore.CurrentValue);
        if (currentScore > highScore)
        {
            _highScore.SetValue(currentScore.ToString());
            Preferences.Set("HighScore", currentScore);
        }
    }
    
    private void CheckHighStreak()
    {
        var currentStreak = int.Parse(_streak.CurrentValue);
        var highStreak = int.Parse(_highStreak.CurrentValue);
        if (currentStreak > highStreak)
        {
            _highStreak.SetValue(currentStreak.ToString());
            Preferences.Set("HighStreak", currentStreak);
        }
    }
}