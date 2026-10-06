using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using PurityUIDashborad.Infrastructure;
using PurityUIDashborad.Views;

#if DEBUG
using Akbura.Diagnostics;
using Avalonia.Input;
#endif

namespace PurityUIDashborad;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var viewModel = AppServices.Current.MainViewModel;
            desktop.MainWindow = new MainWindow
            {
                DataContext = viewModel,
                Content = new AppShell
                {
                    Vm = viewModel,
                    DataContext = viewModel
                }
            };

            desktop.Exit += (_, _) => AppServices.DisposeOwnedServices();

#if DEBUG
            // Desktop Developer Tools: F12; Akbura component inspector: Ctrl+F12.
            this.AttachDeveloperTools();
            this.AttachAkburaDevTools(options =>
            {
                options.ToggleGesture = new KeyGesture(Key.F12, KeyModifiers.Control);
            });
#endif
        }
        else if (ApplicationLifetime is IActivityApplicationLifetime activity)
        {
            var viewModel = AppServices.Current.MainViewModel;
            // Android can recreate an Activity. Each invocation must receive a fresh visual tree.
            activity.MainViewFactory = () => new AppShell
            {
                Vm = viewModel,
                DataContext = viewModel
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleView)
        {
            var viewModel = AppServices.Current.MainViewModel;
            singleView.MainView = new AppShell
            {
                Vm = viewModel,
                DataContext = viewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
