using BlazBeaver.Data;
using BlazBeaver.Interfaces;
using Microsoft.Extensions.Options;

namespace BlazBeaver.Services;

public class IdProvider: IIdProvider
{
    private readonly IApiClient _apiClient;
    private readonly IOptions<AppSettingsOptions> _configuration;

    public IdProvider(IApiClient apiClient, IOptions<AppSettingsOptions> configuration)
    {
        _apiClient = apiClient;
        _configuration = configuration;
    }

    public async Task<string> GetNewIdAsync(IEnumerable<Folder> requirementsInFolders)
    {
        int newId = -1;
        if (_configuration.Value.UseApiForUniqueId && !string.IsNullOrEmpty(_configuration.Value.APIUrl))
        {
            newId = await GetIdFromApi();
        }

        if (newId == -1)
        {
            return Helpers.RequirementsHelper.GetNewId(requirementsInFolders, _configuration.Value.RequirementIdentifier);
        }

        return Helpers.RequirementsHelper.FormatRequirement(newId, _configuration.Value.RequirementIdentifier);
    }

    private async Task<int> GetIdFromApi()
    {
        try
        {
            NewIdResult result = await _apiClient.GetNewIdAsync();
            if (string.Compare(result.Prefix, _configuration.Value.RequirementIdentifier) == 0)
            {
                return result.NewId;
            }
        }
        catch
        {
            Console.WriteLine("Error when trying to get the new Id from the API");
        }

        return -1;
    }
}
