using System.Text;
using System.Text.Json;
using WinUi.Essentials.Contracts;

namespace WinUi.Essentials.Services;

public sealed class FileService : IFileService
{
    public static IFileService Default { get; } = new FileService();

    public T? Read<T>(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
            return default;

        if (!File.Exists(filePath))
            return default;

        using var fileStream = File.OpenRead(filePath);
        return JsonSerializer.Deserialize<T>(fileStream);
    }

    public void Save<T>(string filePath, T content)
    {
        if (string.IsNullOrEmpty(filePath))
            return;

        var directory = Path.GetDirectoryName(filePath)!;

        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        var stringContent = JsonSerializer.Serialize(content);
        File.WriteAllText(filePath, stringContent, Encoding.UTF8);
    }

    public void Delete(string filePath)
    {
        if (!File.Exists(filePath))
            return;

        File.Delete(filePath);
    }
}