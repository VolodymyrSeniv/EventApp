namespace MauiAppB.Views;

public partial class CreateGroupPage : ContentPage
{
	public CreateGroupPage()
	{
		InitializeComponent();
	}
    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}