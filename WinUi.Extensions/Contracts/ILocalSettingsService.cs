namespace WinUi.Extensions.Contracts;

public interface ILocalSettingsService
{
    string? ReadSetting(string key);
    void SaveSetting(string key, string value);
}