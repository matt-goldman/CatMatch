using CatMatch.Models;
using CatMatch.Popups;
using CatMatch.Services;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CatMatch.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly ICatService _catService;
    private readonly IScoreService _scoreService;

    public ObservableCollection<CatCardViewModel> Cats { get; set; } = new();

    [ObservableProperty]
    private string _buttonText;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private double _opacity;

    [ObservableProperty]
    private Cat? _matchedCat;

    [ObservableProperty]
    private bool _matchedCatVisible;

    [ObservableProperty]
    private string _timer = string.Empty;

    [ObservableProperty]
    private string _score = string.Empty;
    
    [ObservableProperty]
    private string _highScore = string.Empty;
    
    [ObservableProperty]
    private string _streak = string.Empty;
    
    [ObservableProperty]
    private string _highStreak = string.Empty;

    private bool _correct = true;

    [ObservableProperty]
    private bool _isHard = false;

    private int _countDown = 10;

    public MainViewModel(ICatService catService, IScoreService scoreService)
    {
        _catService = catService;
        _scoreService = scoreService;
        ButtonText = "Begin";
        Opacity = 1;
        SubscribeToScore();
    }

    private void SubscribeToScore()
    {
        _scoreService.Score.Subscribe(score => Score = score);
        _scoreService.HighScore.Subscribe(highScore => HighScore = highScore);
        _scoreService.Streak.Subscribe(streak => Streak = streak);
        _scoreService.HighStreak.Subscribe(highStreak => HighStreak = highStreak);
    }

    public async Task InitializeAsync()
    {
        IsBusy = true;

        await NextGame();

        Opacity = 0.5f;

        ButtonText = "New game";
    }

    [RelayCommand]
    public async Task NextGame()
    {
        _correct = true;
        MatchedCatVisible = false;
        await FetchCats();

        // todo: move the starting number of seconds to config
        for (int i = _countDown; i > 0; i--)
        {
            Timer = i.ToString();
            await Task.Delay(1000);
        }

        foreach (var cat in Cats)
        {
            cat.ShowAnimation = !cat.ShowAnimation;
            cat.ShowPlaceholder = true;
        }

        var rnd = new Random();
        MatchedCat = Cats[rnd.Next(0, Cats.Count)].Cat;

        MatchedCatVisible = true;
    }

    [RelayCommand]
    private async Task SelectCat(CatCardViewModel catVm)
    {
        catVm.ShowAnimation = !catVm.ShowAnimation;

        await Task.Delay(150);

        catVm.ShowPlaceholder = false;
        
        if (MatchedCat == catVm.Cat)
        {
            await App.Current.MainPage.ShowPopupAsync(new ResultPopup(true));

            if (_correct)
            {
                _scoreService.IncrementScore();
            }

            await NextGame();
        }
        else
        {
            _correct = false;
            _scoreService.ReserStreak();
            
            catVm.IsIncorrect = true;

            await App.Current.MainPage.ShowPopupAsync(new ResultPopup(false));
        }

    }

    private async Task FetchCats()
    {
        IsBusy = true;

        var cats = await _catService.GetCatsAsync();

        Cats.Clear();

        foreach (var cat in cats)
        {
            Cats.Add(new CatCardViewModel { Cat = cat });
        }

        IsBusy = false;
    }
    
    partial void OnIsHardChanged(bool value)
    {
        if (value)
        {
            _countDown = 5;
        }
        else
        {
            _countDown = 10;
        }
    }
}
