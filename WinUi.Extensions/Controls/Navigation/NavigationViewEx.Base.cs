using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Data;

namespace WinUi.Extensions.Controls.Navigation;

public sealed partial class NavigationViewEx : NavigationView, INavigation
{
    public static readonly DependencyProperty FrameProperty = DependencyProperty.Register(
        nameof(Frame),
        typeof(Frame),
        typeof(NavigationViewEx),
        new PropertyMetadata(null));

    public static readonly DependencyProperty SettingPageTypeProperty = DependencyProperty.Register(
        nameof(SettingPageType),
        typeof(Type),
        typeof(NavigationViewEx),
        new PropertyMetadata(null));

    public Frame Frame
    {
        get => (Frame)GetValue(FrameProperty);
        set => SetValue(FrameProperty, value);
    }

    public Type SettingPageType
    {
        get => (Type)GetValue(SettingPageTypeProperty);
        set => SetValue(SettingPageTypeProperty, value);
    }

    public event NavigatedEventHandler? Navigated;

    private readonly Dictionary<string, Type> _tagToPageTypeDictionary = new();
    private readonly Dictionary<Type, string> _pageTypeToTagDictionary = new();
    private readonly Dictionary<Type, NavigationViewItem> _pageTypeToNavigationViewItemsDictionary = new();

    private readonly BreadcrumbBar _breadcrumbBar = new();
    private readonly ObservableCollection<BreadcrumbBarItem> _breadcrumbBarItems = new();

    private Type _currentNavigationViewItemType = typeof(NavigationViewEx);

    public NavigationViewEx()
    {
        Loaded += OnLoaded;
        ItemInvoked += OnItemInvoked;
        BackRequested += OnBackRequested;
        Unloaded += OnUnloaded;
    }

    public void RegisterPage<TPage>(string tag) where TPage : Page
    {
        var type = typeof(TPage);

        _tagToPageTypeDictionary.TryAdd(tag, type);
        _pageTypeToTagDictionary.TryAdd(type, tag);
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        Guard.IsNotNull(Frame);
        Frame.Navigated += FrameOnNavigated;

        foreach (var menuItem in MenuItems)
        {
            if (menuItem is not NavigationViewItemEx navigationViewItem)
                continue;

            Guard.IsNotNull(navigationViewItem.NavigateToType);
            Guard.IsNotNullOrEmpty(navigationViewItem.Tag);

            _tagToPageTypeDictionary.TryAdd(navigationViewItem.Tag, navigationViewItem.NavigateToType);
            _pageTypeToNavigationViewItemsDictionary.TryAdd(navigationViewItem.NavigateToType, navigationViewItem);
        }

        if (IsSettingsVisible)
        {
            Guard.IsNotNull(SettingPageType);

            var settingNavigationViewItem = (NavigationViewItem)SettingsItem;
            var tag = (string)settingNavigationViewItem.Content;

            _tagToPageTypeDictionary.TryAdd(tag, SettingPageType);
            _pageTypeToNavigationViewItemsDictionary.TryAdd(SettingPageType, settingNavigationViewItem);
        }

        WinUi.Extensions.Navigation.Current = this;
        InitializeBreadcrumb();

        var item = (NavigationViewItemEx)MenuItems[0];
        NavigateTo(item.Tag);
    }

    private void InitializeBreadcrumb()
    {
        _breadcrumbBar.Resources.Add("BreadcrumbBarChevronFontSize", 16);
        _breadcrumbBar.Resources.Add("BreadcrumbBarChevronPadding", new Thickness(12, 0, 12, 0));

        _breadcrumbBar.Resources.Add("BreadcrumbBarItemThemeFontSize", 28);
        _breadcrumbBar.Resources.Add("BreadcrumbBarItemFontWeight", 500);

        _breadcrumbBar.Resources.Add("BreadcrumbBarNormalForegroundBrush", Application.Current.Resources["BreadcrumbBarHoverForegroundBrush"]);
        _breadcrumbBar.Resources.Add("BreadcrumbBarHoverForegroundBrush", Application.Current.Resources["BreadcrumbBarCurrentNormalForegroundBrush"]);

        var binding = new Binding { Source = _breadcrumbBarItems };
        _breadcrumbBar.SetBinding(BreadcrumbBar.ItemsSourceProperty, binding);
        _breadcrumbBar.ItemClicked += BreadcrumbBarOnItemClicked;

        Header = _breadcrumbBar;
    }

    private void OnBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args) => GoBack();

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        Loaded -= OnLoaded;
        Frame.Navigated -= FrameOnNavigated;
        BackRequested -= OnBackRequested;
        _breadcrumbBar.ItemClicked -= BreadcrumbBarOnItemClicked;
        Unloaded -= OnUnloaded;
    }
}