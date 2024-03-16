using CatMatch.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CatMatch.ViewModels;

public partial class CatCardViewModel : ObservableObject
{
    public Cat Cat { get; set; }

    [ObservableProperty]
    private bool _showPlaceholder = false;
    
    [ObservableProperty]
    private bool _isIncorrect = false;

    [ObservableProperty]
    private bool _showAnimation = false;
}
