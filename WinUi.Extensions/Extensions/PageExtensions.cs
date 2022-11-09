namespace WinUi.Extensions.Extensions;

public static class PageExtensions
{
    #region HeaderMode

    public static readonly DependencyProperty HeaderModeProperty =
        DependencyProperty.RegisterAttached("HeaderMode", typeof(NavigationViewExHeaderMode),
            typeof(PageExtensions),
            new PropertyMetadata(NavigationViewExHeaderMode.Always));

    public static NavigationViewExHeaderMode GetHeaderMode(this Page item) =>
        (NavigationViewExHeaderMode)item.GetValue(HeaderModeProperty);

    public static void SetHeaderMode(this Page item, NavigationViewExHeaderMode value) =>
        item.SetValue(HeaderModeProperty, value);

    #endregion

    #region CustomHeader

    public static readonly DependencyProperty CustomHeaderProperty =
        DependencyProperty.RegisterAttached("CustomHeader", typeof(string),
            typeof(PageExtensions),
            new PropertyMetadata(string.Empty));

    public static string GetCustomHeader(this Page page) => (string)page.GetValue(CustomHeaderProperty);

    public static void SetCustomHeader(this Page page, string value) => page.SetValue(CustomHeaderProperty, value);

    #endregion


    public static object? GetViewModel(this Page page) => page.DataContext;
}