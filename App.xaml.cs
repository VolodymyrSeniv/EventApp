namespace MauiAppB;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

        // ЗАПУСКАЕМСЯ С ЛОГИНА
        // NavigationPage нужна, чтобы работали переходы (PushAsync)
MainPage = new NavigationPage(new MauiAppB.MainPage());

	}
}

	// protected override Window CreateWindow(IActivationState? activationState)
	// {
	// 	return new Window(new AppShell());
	// }