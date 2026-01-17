using MauiAppB.Views; 
using Microsoft.Maui.Controls;
// Эти строки станут активными только когда вы напишете строку On<iOS>... ниже
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;

namespace MauiAppB;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
    }

    // Переход на ЛОГИН
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
    }

    // Переход на РЕГИСТРАЦИЮ
    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegisterPage());
    }
}