using Microsoft.Extensions.Hosting;
using WinUi.Essentials.Services;

namespace WinUi.Essentials.Demo;

public partial class App : ApplicationEx
{
    public App()
    {
        this.InitializeComponent();

        LocalSettingsService.LocalSettingsDirectoryName = "WinUi.Essentials.Demo";
    }

    protected override IHost InitializeHost()
    {
        var host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder();

        return host.Build();
    }

    protected override Page InitializeMainPage()
    {
        return new AppShell();
    }
}