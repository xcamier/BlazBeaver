namespace BlazBeaver.Interfaces;

public interface IBasicsSettingsRepository
{
    IEnumerable<string> GetAll(string fileName);
}
