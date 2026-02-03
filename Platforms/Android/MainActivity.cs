using Android.App;
using Android.Content.PM;
using Android.OS;
[assembly: MetaData("com.google.android.geo.API_KEY", Value = "AIzaSyBKv_xt6gFKDK_kpTvc5htQG2PK2DyCOK0")]
namespace MauiAppB;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
}
