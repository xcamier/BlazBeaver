namespace BlazBeaver.Interfaces;

public interface IDataIO
{
    IEnumerable<string> LoadAll(string url, string fileExtension);
    string LoadItem(string url);
    bool SaveItem(string oldUrl, string newUrl, string updatedReq);
    string CreateFolder(string newFolderName, string parentFolderName);
    string RenameFolder(string newFolderName, string folderCurrentUrl);
    bool DeleteItem(string url);
}
