namespace MauiAppB.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        // Сразу переходим к Группам
        await Navigation.PushAsync(new GroupsPage());
    }
}