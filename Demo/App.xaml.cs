using Microsoft.Extensions.Hosting;
using WinUi.Extensions;

namespace Demo;

public partial class App : ApplicationEx
{
    public App()
    {
        this.InitializeComponent();
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