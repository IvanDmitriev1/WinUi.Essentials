namespace WinUi.Essentials.Extensions;

public static class FrameExtensions
{
    public static Page? GetContentAsPage(this Frame frame) => frame.Content as Page;
}