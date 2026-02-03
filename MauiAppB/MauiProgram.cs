using Microsoft.Extensions.Logging;
using MauiAppB.Services; // <--- Не забудь добавить этот using наверху!
using MauiAppB.Views;

namespace MauiAppB;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiMaps()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif
		builder.Services.AddSingleton<IDataService, MockDataService>();
		builder.Services.AddTransient<MainPage>();
		return builder.Build();
	}
}
