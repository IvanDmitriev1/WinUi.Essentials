using Windows.Storage.Pickers;
using WinUIEx;

namespace WinUi.Essentials.Extensions;

public static class FileOpenPickerExtensions
{
    public static FileOpenPicker CreateFileOpenPicker()
    {
        var picker = new FileOpenPicker();
        WinRT.Interop.InitializeWithWindow.Initialize(picker, ApplicationEx.MainWindow.GetWindowHandle());

        return picker;
    }
}