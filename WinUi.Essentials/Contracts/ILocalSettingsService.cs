namespace WinUi.Essentials.Contracts;

public interface ILocalSettingsService
{
    string ReadSetting(string key, string defaultValue);
    void SaveSetting(string key, string value);
}