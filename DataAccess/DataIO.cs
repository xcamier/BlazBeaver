using System.Text;
using Microsoft.Extensions.Options;
using BlazBeaver.Interfaces;
using BlazBeaver.Data;

namespace BlazBeaver.DataAccess;

public class DataIO : IDataIO
{
    private readonly IOptions<AppSettingsOptions> _configuration;

    public DataIO(IOptions<AppSettingsOptions> configuration)
    {
        _configuration = configuration;
    }

    public IEnumerable<string> LoadAll(string url, string fileExtension)
    {
        List<string> allFoldersAndFiles = new List<string>();

        //Gets the files for the requirements folder
        IEnumerable<string> foldersAndFiles = Browse(url, fileExtension);

        return foldersAndFiles;
    }

    public string LoadItem(string url)
    {
        try
        {
            if (File.Exists(url))
            {
                using (StreamReader sr = new StreamReader(url))
                {
                    return sr.ReadToEnd();
                }
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

    public bool SaveItem(string oldUrl, string newUrl, string updatedReq)
    {
        try
        {
            using (FileStream fs = File.OpenWrite(newUrl))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(updatedReq);

                fs.Write(bytes, 0, bytes.Length);
            }

            if (!string.IsNullOrEmpty(oldUrl) && oldUrl != newUrl)
            {
                File.Delete(oldUrl);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    public string CreateFolder(string newFolderName, string parentUrl)
    {
        try
        {
            string path = parentUrl;
            if (string.IsNullOrEmpty(path))
            {
                path = _configuration.Value.RequirementsFolderLocation;
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
