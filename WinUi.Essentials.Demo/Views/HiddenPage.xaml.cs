using Microsoft.Extensions.DependencyInjection;
using WinUi.Essentials.Demo.ViewModels;

namespace WinUi.Essentials.Demo.Views;

public sealed partial class HiddenPage
{
    public HiddenPage()
    {
        this.InitializeComponent();

        var viewModel = ApplicationEx.ServiceProvider.GetRequiredService<HiddenPageViewModel>();
        DataContext = viewModel;
        ViewModel = viewModel;
    }

    public HiddenPageViewModel ViewModel { get; }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Navigation.Current!.NavigateTo(nameof(BlankPage2));
    }

    private void HiddenPage_OnUnloaded(object sender, RoutedEventArgs e)
    {
        ViewModel.IsActive = false;
    }

    private void HiddenPage_OnNavigated(object? sender, EventArgs e)
    {
        ViewModel.IsActive = true;
    }
}