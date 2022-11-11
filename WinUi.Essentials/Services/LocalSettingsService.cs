using System.Text.Json;
using Windows.Storage;
using WinUi.Essentials.Contracts;
using WinUi.Essentials.Helpers;

namespace WinUi.Essentials.Services;

public static class LocalSettingsService
{
    private static string _localSettingsDirectoryName = string.Empty;

    public static string LocalSettingsDirectoryName
    {
        get => _localSettingsDirectoryName;
        set
        {
            _localSettingsDirectoryName = value;
            Default = new UnpackagedLocalSettingsService(value);
        }
    }

    public static ILocalSettingsService Default { get; private set; } = RuntimeHelper.IsMSIX
        ? new PackagedLocalSettingsService()
        : new UnpackagedLocalSettingsService(LocalSettingsDirectoryName);
}

public sealed class UnpackagedLocalSettingsService : ILocalSettingsService
{
    public UnpackagedLocalSettingsService(string appName)
    {
        _localSettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            appName, DefaultLocalSettingsFile);

        _fileService = FileService.Default;

        _settings = _fileService.Read<Dictionary<string, string>>(_localSettingsPath) ??
                    new Dictionary<string, string>();
    }

    private const string DefaultLocalSettingsFile = "LocalSettings.json";

    private readonly string _localSettingsPath;
    private readonly IFileService _fileService;
    private readonly Dictionary<string, string> _settings;

    public string? ReadSetting(string key)
    {
        if (_settings.TryGetValue(key, out var obj))
        {
            return obj;
        }

        return default;
    }

    public void SaveSetting(string key, string value)
    {
        if (_settings.ContainsKey(key))
        {
            _settings[key] = value;
        }
        else
        {
            _settings.Add(key, value);
        }

        _fileService.Save(_localSettingsPath, _settings);
    }
}

public sealed class PackagedLocalSettingsService : ILocalSettingsService
{
    public string? ReadSetting(string key)
    {
        if (ApplicationData.Current.LocalSettings.Values.TryGetValue(key, out var obj))
        {
            return JsonSerializer.Deserialize<string>((string)obj);
        }

        return default;
    }

    public void SaveSetting(string key, string value)
    {
        ApplicationData.Current.LocalSettings.Values[key] = value;
    }
}