using Microsoft.Extensions.DependencyInjection;

namespace WinUi.Extensions;

public abstract class BasePage<TViewModel> : Page where TViewModel : class
{
    protected BasePage()
    {
        var viewModel = ApplicationEx.ServiceProvider.GetRequiredService<TViewModel>();

        ViewModel = viewModel;
        DataContext = viewModel;
    }

    public TViewModel ViewModel { get; }
}