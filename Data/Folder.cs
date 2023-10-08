using BlazBeaver.Interfaces;

namespace BlazBeaver.Data;

public class Folder
{
    public string Guid { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public List<IReqProt> FolderItems { get; set; } = new List<IReqProt>();
    public List<Folder> SubFolders { get; set; } = new List<Folder>();

    public string FolderName 
    {
        get 
        {
            string[] allParts = Url.Split('/');
            return allParts[allParts.Length - 1];
        }
    }
}
