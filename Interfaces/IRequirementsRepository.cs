using BlazBeaver.Data;

namespace BlazBeaver.Interfaces
{
    public interface IRequirementsRepository
    {
        IEnumerable<Folder> GetAllRequirements();
        Requirement GetRequirement(string reqId);
        bool AddRequirement(Requirement req);
        bool UpdateRequirement(Requirement req);
        bool DeleteRequirement(string url);
        bool DeleteFolder(string url);
        string CreateFolder(string newFolderName, string parentFolderName);
        string RenameFolder(string newFolderName, string folderCurrentUrl);
    }
}