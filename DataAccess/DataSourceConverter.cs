using BlazBeaver.Data;
using BlazBeaver.Interfaces;

namespace BlazBeaver.DataAccess;

public class DataSourceConverter<T> where T: IReqProt, new()
{
    public IEnumerable<Folder> Convert(IEnumerable<string> folderContent)
    {
        List<Folder> folders = new List<Folder>();

        Folder currentFolder = AddAsNewFolder(string.Empty, Helpers.ReqAndProcProperties.RootFolderName, folders);
        foreach(string item in folderContent)
        {   
            string ext = Path.GetExtension(item);
            if (ext.CompareTo(".md") == 0)
            {
                //It's a req
                T req = new T()
                {
                    Url = item
                };
                
                currentFolder.FolderItems.Add(req);    
            }
            else
            {
                //It's a folder
                currentFolder = AddAsNewFolder(string.Empty, item, folders);
            }
        }

        return folders;
    } 

    private Folder AddAsNewFolder(string guid, string url, List<Folder> folders)
    {
        Folder newFolder = new Folder()
        {
            Guid = guid,
            Url = url
        };
        //if this folder contains the name of another folder => adds this one as subfolder of the other one
        AttachToParentFolder(folders, newFolder);

        return newFolder;
    }

    private void AttachToParentFolder(List<Folder> folders, Folder currentFolder)
    {
        if (currentFolder.Url == Helpers.ReqAndProcProperties.RootFolderName)
        {
            //root
            folders.Add(currentFolder);
        }
        else
        {
            string parentFolderUrl = Path.GetDirectoryName(currentFolder.Url);
            Folder fd = Helpers.FolderHelpers.FindFolderFromUrl(parentFolderUrl, folders.ElementAt(0));

            if (fd == null)
            {
                //root
                fd = folders.ElementAt(0);
            }
            fd.SubFolders.Add(currentFolder);
        }
    }   
}

