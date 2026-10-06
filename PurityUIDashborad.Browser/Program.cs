using Avalonia;
using Avalonia.Browser;
using PurityUIDashborad;
using PurityUIDashborad.Infrastructure;
using System.Threading.Tasks;

internal sealed partial class Program
{
    private static Task Main(string[] args) => BuildAvaloniaApp()
            .StartBrowserAppAsync("out");

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UseAkburaApplication();
}
