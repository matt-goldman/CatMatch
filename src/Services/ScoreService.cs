using CatMatch.Helpers;

namespace CatMatch.Services;

public interface IScoreService
{
    State<string> Score { get; }

    void IncrementScore();
}

public class ScoreService : IScoreService
{
    private State<string> _score;

    public ScoreService()
    {
        _score = new State<string>("0");
    }

    public State<string> Score => _score;

    public void IncrementScore()
    {
        var currentScore = int.Parse(_score.CurrentValue);
        _score.SetValue((currentScore + 1).ToString());
    }
}