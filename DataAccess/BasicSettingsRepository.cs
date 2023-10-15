using BlazBeaver.Interfaces;

namespace BlazBeaver.DataAccess;

public class BasicSettingsRepository : IBasicsSettingsRepository
{
    private readonly IDataIO _dataIO;

    public BasicSettingsRepository(IDataIO dataIO)
    {
        _dataIO = dataIO;
    }

    public IEnumerable<string> GetAll(string fileName)
    {
        List<string> allItems = new List<string>();

        string path = Path.Combine(Helpers.FilesSettings.ResourcesLocation, fileName);
        string fileContent = _dataIO.LoadItem(path);  

        if (!string.IsNullOrEmpty(fileContent))
        {
            allItems = fileContent.Split('\n').ToList();
        }   
        
        return allItems;
    }
}
