using Microsoft.Maui.Controls;
using MauiAppB.Models;
using MauiAppB.Services;
namespace MauiAppB;

public partial class App : Application
{
    public static User CurrentUser { get; set; }
	public App()
	{
		InitializeComponent();

        // ЗАПУСКАЕМСЯ С ЛОГИНА
        // NavigationPage нужна, чтобы работали переходы (PushAsync)
        var navigationPage = new NavigationPage(new MauiAppB.MainPage());
        navigationPage.Padding = new Thickness(0);
        navigationPage.BackgroundColor = Colors.Transparent;
        MainPage = navigationPage;

	}

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return base.CreateWindow(activationState);
    }
}

	// protected override Window CreateWindow(IActivationState? activationState)
	// {
	// 	return new Window(new AppShell());
	// }