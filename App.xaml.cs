using Microsoft.Maui.Controls;
using MauiAppB.Services;

using System.Security.Cryptography.X509Certificates;
namespace MauiAppB;

public partial class App : Application
{
    public static GroupService GroupService { get; private set; }
    public static EventService EventService { get; private set; }
    public App(GroupService groupService, EventService eventService)
	{
		InitializeComponent();
        GroupService = groupService;
        EventService = eventService; // Zainicjalizuj serwis
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