using MauiAppB.Views;

namespace MauiAppB;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(GroupDetailsPage), typeof(GroupDetailsPage));
	}
}
