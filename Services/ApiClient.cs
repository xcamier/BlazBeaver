using System.Net.Http.Headers;
using BlazBeaver.Data;
using BlazBeaver.Interfaces;

namespace BlazBeaver.Services;

public class ApiClient : IApiClient
{
    private static HttpClient _client = new HttpClient();

    public ApiClient()
    {
        _client.BaseAddress = new Uri(Helpers.AppSettings.APIUrl);
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<NewIdResult> GetNewIdAsync()
    {
        NewIdResult newIdResult = null;

        HttpResponseMessage response = await _client.GetAsync(Helpers.ReqAndProcProperties.RequirementIdentifier);
        if (response.IsSuccessStatusCode)
        {
            newIdResult = await response.Content.ReadAsAsync<NewIdResult>();
        }

        return newIdResult;
    }

}
