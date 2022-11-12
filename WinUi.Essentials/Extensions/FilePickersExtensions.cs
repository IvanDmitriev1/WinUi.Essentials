using Windows.Storage.Pickers;
using WinUIEx;

namespace WinUi.Essentials.Extensions;

public static class FilePickersExtensions
{
    public static FileOpenPicker CreateFileOpenPicker(string fileType)
    {
        var picker = new FileOpenPicker();
        WinRT.Interop.InitializeWithWindow.Initialize(picker, ApplicationEx.MainWindow.GetWindowHandle());
        picker.FileTypeFilter.Add(fileType);

        return picker;
    }

    public static FileSavePicker CreateFileSavePicker(string suggestedFileName, string key, IList<string> values)
    {
        var picker = new FileSavePicker();
        WinRT.Interop.InitializeWithWindow.Initialize(picker, ApplicationEx.MainWindow.GetWindowHandle());
        picker.SuggestedFileName = suggestedFileName;
        picker.FileTypeChoices.Add(key, values);

        return picker;
    }
}