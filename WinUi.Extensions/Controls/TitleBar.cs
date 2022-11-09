using Microsoft.UI.Xaml.Media;
using WinUi.Extensions.Helpers;

namespace WinUi.Extensions.Controls;

[TemplatePart(Name = "AppTitleBar", Type = typeof(FrameworkElement))]
[TemplatePart(Name = "AppTitleBarText", Type = typeof(TextBlock))]
public sealed class TitleBar : ContentControl
{
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        nameof(Title), typeof(string),
        typeof(TitleBar), new PropertyMetadata("App title"));

    public static readonly DependencyProperty NavigationViewProperty = DependencyProperty.Register(
        nameof(NavigationView), typeof(NavigationView),
        typeof(TitleBar), new PropertyMetadata(null));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public NavigationView? NavigationView
    {
        get => (NavigationView)GetValue(NavigationViewProperty);
        set => SetValue(NavigationViewProperty, value);
    }

    public TitleBar()
    {
        this.DefaultStyleKey = typeof(TitleBar);

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    private FrameworkElement _appTitlebar = null!;
    private TextBlock _appTitleBarText = null!;

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _appTitlebar = (FrameworkElement)GetTemplateChild("AppTitleBar");
        _appTitleBarText = (TextBlock)GetTemplateChild("AppTitleBarText");
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        ApplicationEx.MainWindow.ExtendsContentIntoTitleBar = true;
        ApplicationEx.MainWindow.SetTitleBar(_appTitlebar);
        ApplicationEx.MainWindow.Activated += MainWindowOnActivated;

        TitleBarHelper.UpdateTitleBar(RequestedTheme);

        if (NavigationView is not null)
        {
            NavigationView.DisplayModeChanged += NavigationViewOnDisplayModeChanged;
            SetAppTitleBarMargin(NavigationView);
        }
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        if (NavigationView is not null)
            NavigationView.DisplayModeChanged -= NavigationViewOnDisplayModeChanged;

        Loaded -= OnLoaded;
        Unloaded -= OnUnloaded;

        ApplicationEx.MainWindow.Activated -= MainWindowOnActivated;
    }

    private void MainWindowOnActivated(object sender, WindowActivatedEventArgs args)
    {
        var resource = args.WindowActivationState == WindowActivationState.Deactivated ? "WindowCaptionForegroundDisabled" : "WindowCaptionForeground";
        _appTitleBarText.Foreground = (SolidColorBrush)Application.Current.Resources[resource];
    }

    private void NavigationViewOnDisplayModeChanged(NavigationView sender, NavigationViewDisplayModeChangedEventArgs args)
    {
        SetAppTitleBarMargin(sender);
    }

    private void SetAppTitleBarMargin(NavigationView navigationView)
    {
        _appTitlebar.Margin = new Thickness
        {
            Left = navigationView.CompactPaneLength *
                   (navigationView.DisplayMode == NavigationViewDisplayMode.Minimal ? 2 : 1),
            Top = _appTitlebar.Margin.Top,
            Right = _appTitlebar.Margin.Right,
            Bottom = _appTitlebar.Margin.Bottom
        };
    }
}