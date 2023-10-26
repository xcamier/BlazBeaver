using Microsoft.Extensions.Options;
using BlazBeaver.Interfaces;
using BlazBeaver.Data;

namespace BlazBeaver.DataAccess;

public class BasicSettingsRepository : IBasicsSettingsRepository
{
    private readonly IDataIO _dataIO;
    private readonly IOptions<AppSettingsOptions> _configuration;
    
    public BasicSettingsRepository(IDataIO dataIO, IOptions<AppSettingsOptions> configuration)
    {
        _dataIO = dataIO;
        _configuration = configuration;        
    }

    public IEnumerable<string> GetAll(string fileName)
    {
        List<string> allItems = new List<string>();

        string path = Path.Combine(_configuration.Value.ResourcesLocation, fileName);
        string fileContent = _dataIO.LoadItem(path);  

        if (!string.IsNullOrEmpty(fileContent))
        {
            allItems = fileContent.Split('\n').ToList();
        }   
        
        return allItems;
    }
}
