using CommunityToolkit.Maui.Views;

namespace CatMatch.Popups;

public partial class ResultPopup : Popup
{
	public ResultPopup(bool won)
	{
		InitializeComponent();
		if (won)
		{
			HappyCat.IsVisible = true;
			SadCat.IsVisible = false;
			Confetti.IsVisible = true;
			CloseButton.Text = "Yay!";
			ResultLabel.Text = "✅ Correct 😍";
		}
		else
		{
			HappyCat.IsVisible = false;
			SadCat.IsVisible = true;
			Confetti.IsVisible = false;
			CloseButton.Text = "Try again";
			ResultLabel.Text = "❌ Incorrect 😭";
		}
	}

    private void CloseButton_Clicked(object sender, EventArgs e)
    {
	    CloseAsync();
    }
}