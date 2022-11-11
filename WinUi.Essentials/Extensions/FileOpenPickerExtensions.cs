using Windows.Storage.Pickers;
using WinUIEx;

namespace WinUi.Essentials.Extensions;

public static class FileOpenPickerExtensions
{
    public static FileOpenPicker CreateFileOpenPicker(string fileType)
    {
        var picker = new FileOpenPicker();
        WinRT.Interop.InitializeWithWindow.Initialize(picker, ApplicationEx.MainWindow.GetWindowHandle());
        picker.FileTypeFilter.Add(fileType);

        return picker;
    }
}