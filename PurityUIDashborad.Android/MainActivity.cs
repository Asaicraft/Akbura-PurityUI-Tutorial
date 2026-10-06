using Android.App;
using Android.Content.PM;
using Avalonia.Android;

namespace PurityUIDashborad.Android;

[Activity(
    Label = "PurityUIDashborad.Android",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity
{
}
