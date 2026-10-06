using Microsoft.Extensions.DependencyInjection;
using PurityUIDashborad.Services;
using PurityUIDashborad.ViewModels;


namespace PurityUIDashborad.Infrastructure;
/// <summary>One composition root shared by all platform hosts and Activity recreations.</summary>
internal sealed class AppServices
{
    private static readonly Lazy<AppServices> s_current = new(() => new AppServices());

    private readonly ServiceProvider _provider;

    private AppServices()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IGreetingService, GreetingService>();
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<IInvoiceService, MockInvoiceService>();

        _provider = services.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateScopes = true,
            ValidateOnBuild = true
        });

        MainViewModel = _provider.GetRequiredService<MainViewModel>();
    }

    public static AppServices Current => s_current.Value;

    public MainViewModel MainViewModel { get; }

    public IServiceProvider ServiceProvider => _provider;

    public static void DisposeOwnedServices()
    {
        if (s_current.IsValueCreated)
        {
            s_current.Value._provider.Dispose();
        }
    }
}
