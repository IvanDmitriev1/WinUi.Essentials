using WinUi.Essentials.Contracts;

namespace WinUi.Essentials;

public sealed class Navigation
{
    public static INavigation? Current { get; internal set; }
}