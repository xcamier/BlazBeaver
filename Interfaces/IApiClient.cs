using BlazBeaver.Data;

namespace BlazBeaver.Interfaces;

public interface IApiClient
{
    Task<NewIdResult> GetNewIdAsync();
}
