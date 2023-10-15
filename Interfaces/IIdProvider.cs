using BlazBeaver.Data;

namespace BlazBeaver.Interfaces;

public interface IIdProvider
{
    Task<string> GetNewIdAsync(IEnumerable<Folder> requirementsInFolders);
}
