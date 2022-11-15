namespace WinUi.Essentials.Extensions;

public static class PageExtensions
{
    public static object? GetViewModel(this Page page) => page.DataContext;
}