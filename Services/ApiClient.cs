using System.Net.Http.Headers;
using BlazBeaver.Data;
using BlazBeaver.Interfaces;
using Microsoft.Extensions.Options;

namespace BlazBeaver.Services;

public class ApiClient : IApiClient
{
    private static HttpClient _client = new HttpClient();
    private readonly IOptions<AppSettingsOptions> _configuration;

    public ApiClient(IOptions<AppSettingsOptions> configuration)
    {
        _configuration = configuration;
        _client.BaseAddress = new Uri(_configuration.Value.APIUrl);
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<NewIdResult> GetNewIdAsync()
    {
        NewIdResult newIdResult = null;

        HttpResponseMessage response = await _client.GetAsync(_configuration.Value.RequirementIdentifier);
        if (response.IsSuccessStatusCode)
        {
            newIdResult = await response.Content.ReadAsAsync<NewIdResult>();
        }

        return newIdResult;
    }

}
