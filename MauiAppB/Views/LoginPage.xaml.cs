using MauiAppB.Models;
using MauiAppB.Services;
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

    private async void OnLoginClicked(object sender, EventArgs e)
{
    string username = UsernameEntry.Text;
    string password = PasswordEntry.Text; // Пока не используем, но поле есть

    var apiService = new ApiService();
    
    // Пытаемся найти пользователя на сервере
    var user = await apiService.LoginUser(username, password);

    if (user != null)
    {
        App.CurrentUser = user;
        // УРА! Мы вошли.
        // Можно сохранить пользователя в глобальную переменную, чтобы знать, кто вошел
        // App.CurrentUser = user; (если сделаешь такое поле в App.xaml.cs)

        // Переходим на главную
        Application.Current.MainPage = new NavigationPage(new GroupsPage());
    }
    else
    {
        await DisplayAlert("Błąd", "Nie znaleziono użytkownika", "OK");
    }
}
}