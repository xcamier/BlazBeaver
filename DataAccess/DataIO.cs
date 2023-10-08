using System.Text;
using BlazBeaver.Interfaces;

namespace BlazBeaver.DataAccess;

public class DataIO : IDataIO
{
    public IEnumerable<string> LoadAll(string url, string fileExtension)
    {
        List<string> allFoldersAndFiles = new List<string>();

        //Gets the files for the requirements folder
        IEnumerable<string> foldersAndFiles = Browse(url, fileExtension);

        return foldersAndFiles;
    }

    public string LoadItem(string url)
    {
        using (StreamReader sr = new StreamReader(url))
        {
            return sr.ReadToEnd();
        }
    }  

    public void SaveItem(string oldUrl, string newUrl, string updatedReq)
    {
        using (FileStream fs = File.OpenWrite(newUrl))
        {
            byte[] bytes = Encoding.UTF8.GetBytes(updatedReq);

            fs.Write(bytes, 0, bytes.Length);
        }

        if (oldUrl != newUrl)
        {
            File.Delete(oldUrl);
        }
    }

    public string CreateFolder(string newFolderName, string parentUrl)
    {
        try
        {
            string path = parentUrl;
            if (string.IsNullOrEmpty(path))
            {
                path = Helpers.FilesSettings.RequirementsFolderLocation;
            }
 
            path = Path.Combine(path, newFolderName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return path;
            }
            else
            {
                return string.Empty;
            }
        }
        catch 
        {
            return string.Empty;
        }
    }

    public string RenameFolder(string newFolderName, string folderCurrentUrl)
    {
        try
        {
            string newPath = Path.GetDirectoryName(folderCurrentUrl);
            newPath = Path.Combine(newPath, newFolderName);

            if (!Directory.Exists(newPath))
            {
                Directory.Move(folderCurrentUrl, newPath);
                return newPath;
            }
            else
            {
                return string.Empty;
            }
        }
        catch 
        {
            return string.Empty;
        }
    }

    public bool DeleteItem(string url)
    {
        try
        {
            if (Directory.Exists(url))
            {
                Directory.Delete(url, true);
            }
            else if (File.Exists(url))
            {
                File.Delete(url);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    private IEnumerable<string> Browse(string startFolder, string fileExtension)
    {
        List<string> foldersAndFiles = new List<string>();

        IEnumerable<string> rootFiles = GetFiles(startFolder, fileExtension);
        foldersAndFiles.AddRange(rootFiles);

        foreach (string dir in Directory.EnumerateDirectories(startFolder, "*.*", SearchOption.AllDirectories))
        {
            foldersAndFiles.Add(dir);

            IEnumerable<string> folderFiles = GetFiles(dir, fileExtension);
            foldersAndFiles.AddRange(folderFiles);
        }

        return foldersAndFiles;
    }

    private IEnumerable<string> GetFiles(string fromFolder, string fileExtension)
    {
        if (Directory.Exists(fromFolder))
        {
            string[] files = Directory.GetFiles(fromFolder, $"*{fileExtension}", SearchOption.TopDirectoryOnly);
            return files.ToList();
        }
        else
            return new List<string>();
    }
}
