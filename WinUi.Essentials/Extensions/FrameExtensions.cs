using WinUi.Essentials.Controls.Navigation;

namespace WinUi.Essentials.Extensions;

public static class FrameExtensions
{
    public static NavigationPage? GetContentAsPage(this Frame frame) => frame.Content as NavigationPage;
}