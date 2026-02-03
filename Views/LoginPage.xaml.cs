using MauiAppB.Services;
using MauiAppB.ViewModels;

namespace MauiAppB.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

    private async void OnBackClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new NavigationPage(new MainPage());
    }

    // C#
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        try
        {
            var authService = Handler.MauiContext.Services.GetService<AuthService>();
            var user = await authService.LoginAsync(EmailEntry.Text, PasswordEntry.Text);

            if (user != null)
            {
                var vm = Handler.MauiContext.Services.GetService<GroupListViewModel>();
                Application.Current.MainPage = new NavigationPage(new GroupsPage(vm));
            }
            else
            {
                await DisplayAlert("Error", "Invalid email or password", "OK");
            }
        }
        catch (System.Net.WebException wex)
        {
            await DisplayAlert("Network error", "Network connection failed. Try again.", "OK");
            // log wex
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "An unexpected error occurred.", "OK");
            // log ex
        }
    }
}