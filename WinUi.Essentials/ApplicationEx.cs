using Microsoft.Extensions.Hosting;
using WinUi.Essentials.Services;
using WinUIEx;

namespace WinUi.Essentials;

public abstract class ApplicationEx : Application
{
    public static WindowEx MainWindow { get; protected set; } = null!;
    public static IServiceProvider ServiceProvider { get; protected set; } = null!;
    public static IHost Host { get; protected set; } = null!;

    protected abstract IHost InitializeHost();
    protected abstract Page InitializeMainPage();

    protected WindowEx OnMainWindowCreating()
    {
        Host = InitializeHost();
        ServiceProvider = Host.Services;

        return new WindowEx
        {
            Backdrop = new MicaSystemBackdrop(),
            Content = InitializeMainPage()
        };
    }

    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        MainWindow = OnMainWindowCreating();
        MainWindow.Closed += MainWindowOnClosed;

        MainWindow.Activate();
        ThemeSelectorService.Default.Initialize();

        await Host.StartAsync();
    }

    private static async void MainWindowOnClosed(object sender, WindowEventArgs args)
    {
        MainWindow.Closed -= MainWindowOnClosed;

        await Host.StopAsync();
    }
}