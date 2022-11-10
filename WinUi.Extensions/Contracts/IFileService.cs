namespace WinUi.Extensions.Contracts;

public interface IFileService
{
    T? Read<T>(string filePath);
    void Save<T>(string filePath, T content);

    void Delete(string filePath);
}