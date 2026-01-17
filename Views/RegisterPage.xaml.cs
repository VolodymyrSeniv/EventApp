namespace MauiAppB.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();
	}

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnNextClicked(object sender, EventArgs e)
    {
        // Просто переходим на Логин
        await Navigation.PushAsync(new LoginPage());
    }
}