using WinUi.Essentials.Enums;

namespace WinUi.Essentials.Controls.Navigation;

public class NavigationPage : Page
{
    public NavigationPage()
    {
        NavigationCacheMode = NavigationCacheMode.Enabled;
    }

    public static readonly DependencyProperty HeaderModeProperty =
        DependencyProperty.Register(nameof(HeaderMode), typeof(NavigationViewExHeaderMode),
            typeof(NavigationPage),
            new PropertyMetadata(NavigationViewExHeaderMode.Always));

    public static readonly DependencyProperty CustomHeaderProperty =
        DependencyProperty.Register(nameof(CustomHeader), typeof(string),
            typeof(NavigationPage),
            new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty RemoveFromHistoryProperty =
        DependencyProperty.Register(nameof(InterceptNavigationFromUser), typeof(bool),
            typeof(NavigationPage),
            new PropertyMetadata(false));

    public NavigationViewExHeaderMode HeaderMode
    {
        get => (NavigationViewExHeaderMode)GetValue(HeaderModeProperty);
        set => SetValue(HeaderModeProperty, value);
    }

    public string CustomHeader
    {
        get => (string)GetValue(CustomHeaderProperty);
        set => SetValue(CustomHeaderProperty, value);
    }

    public bool InterceptNavigationFromUser
    {
        get => (bool)GetValue(RemoveFromHistoryProperty);
        set => SetValue(RemoveFromHistoryProperty, value);
    }
}