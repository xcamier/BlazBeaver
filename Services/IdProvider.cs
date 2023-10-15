using BlazBeaver.Data;
using BlazBeaver.Interfaces;

namespace BlazBeaver.Services;

public class IdProvider: IIdProvider
{
    IApiClient _apiClient;

    public IdProvider(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<string> GetNewIdAsync(IEnumerable<Folder> requirementsInFolders)
    {
        int newId = -1;
        if (Helpers.AppSettings.UseApiForUniqueId && !string.IsNullOrEmpty(Helpers.AppSettings.APIUrl))
        {
            newId = await GetIdFromApi();
        }

        if (newId == -1)
        {
            return Helpers.RequirementsHelper.GetNewId(requirementsInFolders);
        }

        return Helpers.RequirementsHelper.FormatRequirement(newId);
    }

    private async Task<int> GetIdFromApi()
    {
        try
        {
            NewIdResult result = await _apiClient.GetNewIdAsync();
            if (string.Compare(result.Prefix, Helpers.ReqAndProcProperties.RequirementIdentifier) == 0)
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
