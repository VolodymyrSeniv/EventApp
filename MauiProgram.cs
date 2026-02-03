using CommunityToolkit.Maui;
using MauiAppB.Services; // <--- Не забудь добавить этот using наверху!
using MauiAppB.ViewModels;
using MauiAppB.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;

namespace MauiAppB;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMauiMaps()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		string dbPath = Path.Combine(FileSystem.AppDataDirectory, "groups.db3");
#if DEBUG
        builder.Logging.AddDebug();
#endif
		builder.Services.AddSingleton<GroupService>(s => ActivatorUtilities.CreateInstance<GroupService>(s, dbPath));
        builder.Services.AddSingleton<EventService>(s =>ActivatorUtilities.CreateInstance<EventService>(s, dbPath));
        builder.Services.AddSingleton<GroupListViewModel>();
		builder.Services.AddTransient<GroupDetailsPage>();
		builder.Services.AddTransient<CreateGroupPage>();
        builder.Services.AddTransient<GroupDetailsViewModel>();
		builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<CreateEventPage>();
        builder.Services.AddTransient<EventListViewModel>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddSingleton<AuthService>();
        return builder.Build();
	}
}
