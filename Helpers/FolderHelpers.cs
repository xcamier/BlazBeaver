using BlazBeaver.Data;

namespace BlazBeaver.Helpers;

public class FolderHelpers
{
    public static Folder FindFolderFromUrl(string url, Folder startPoint)
    {
        if (startPoint.Url == url)
        {
            return startPoint;
        }
        else
        {
            foreach (Folder fd in startPoint.SubFolders)
            {
                Folder foundFolder = FindFolderFromUrl(url, fd);
                if (foundFolder != null)
                {
                    return foundFolder;
                }
            }
        }

        return null;
    }    
}
