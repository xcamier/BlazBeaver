using BlazBeaver.Data;
using Microsoft.AspNetCore.Components;

namespace BlazBeaver.Interfaces
{
    public interface IRequirementsRepository
    {
        event EventHandler OnStartLoadingRequirements;
        event EventHandler OnEndLoadingRequirements;

        IEnumerable<Folder> GetAllRequirements();
        Requirement GetRequirement(string reqId);
        IEnumerable<Folder> GetAllCachedRequirements();
        string SaveRequirement(Requirement req, string folderUrl);
        bool DeleteRequirement(string url);
        bool DeleteFolder(string url);
        string CreateFolder(string newFolderName, string parentFolderName);
        string RenameFolder(string newFolderName, string folderCurrentUrl);
    }
}