using WinUi.Essentials.Demo.Views;

namespace WinUi.Essentials.Demo;

public sealed partial class AppShell : Page
{
    public AppShell()
    {
        this.InitializeComponent();

        NavigationViewControl.RegisterPage<HiddenPage>(nameof(HiddenPage));
    }
}