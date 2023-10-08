using BlazBeaver.Data;
using BlazBeaver.Interfaces;

namespace BlazBeaver.DataAccess;

public class RequirementsRepository : IRequirementsRepository
{
    IEnumerable<Folder> _allRequirementsFolders;

    IDataIO _dataIO;
    DataSourceConverter<Requirement> _converter; 

    public RequirementsRepository(IDataIO dataIO, DataSourceConverter<Requirement> converter)
    {
        _dataIO = dataIO;
        _converter = converter;
    }

    public bool AddRequirement(Requirement req)
    {
        throw new NotImplementedException();
    }

    public bool DeleteRequirement(string url)
    {
        bool deletionSuccessful = _dataIO.DeleteItem(url);

        if (deletionSuccessful)
        {
            Folder folder = FindParentFolder(url, _allRequirementsFolders.ElementAt(0), true);
            if (folder != null)
            {
                bool localDeletionSuccessful = false;
                var itemToRemove = folder.FolderItems.FirstOrDefault(fi => fi.Url == url);
                if (itemToRemove != null)
                {
                    localDeletionSuccessful = folder.FolderItems.Remove(itemToRemove);
                }

                //Force the cache loading if could remove the items "programmatically"
                if (!localDeletionSuccessful)
                {
                    GetAllRequirements();
                }
            }
        }

        return deletionSuccessful;
    }

    public bool DeleteFolder(string url)
    {
        bool deletionSuccessful = _dataIO.DeleteItem(url);

        if (deletionSuccessful)
        {
            DeleteFolderFromCache(url);
        }

        return deletionSuccessful;
    }

    public IEnumerable<Folder> GetAllRequirements()
    {
        IEnumerable<string> rawRequirements = _dataIO.LoadAll(Helpers.FilesSettings.RequirementsFolderLocation, Helpers.FilesSettings.FileExtension);
        IEnumerable<Folder> requirements = _converter.Convert(rawRequirements);
        
        _allRequirementsFolders = requirements;

        return requirements;
    }

    public Requirement GetRequirement(string reqId)
    {
        throw new NotImplementedException();
    }

    public bool UpdateRequirement(Requirement req)
    {
        throw new NotImplementedException();
    }

    public string CreateFolder(string newFolderName, string parentUrl)
    {
        //if root folder, there is no parent
        string fixedUrl = parentUrl == Helpers.ReqAndProcProperties.RootFolderName ? string.Empty : parentUrl;

        string newFolderUrl = _dataIO.CreateFolder(newFolderName, fixedUrl);

        if (!string.IsNullOrEmpty(newFolderUrl))
        {
            Folder startFolder = _allRequirementsFolders.ElementAt(0);

            if (fixedUrl == string.Empty)
            {
                AddNewFolderInFolder(string.Empty, newFolderUrl, startFolder);
            }
            else
            {
                Folder foundFolder = Helpers.FolderHelpers.FindFolderFromUrl(fixedUrl, startFolder);
                if (foundFolder == null)
                {
                    GetAllRequirements();
                }
                else
                {
                    AddNewFolderInFolder(string.Empty, newFolderUrl, foundFolder);
                }
            }
        }
        else
        {
            return string.Empty;
        }

        return newFolderUrl;
    }

    public string RenameFolder(string newFolderName, string folderCurrentUrl)
    {
        string newUrlOfFlder = _dataIO.RenameFolder(newFolderName, folderCurrentUrl);

        Folder currentFolderInCache = Helpers.FolderHelpers.FindFolderFromUrl(folderCurrentUrl, _allRequirementsFolders.ElementAt(0));

        if (currentFolderInCache != null)
        {
            currentFolderInCache.Url = newUrlOfFlder;
            return newUrlOfFlder;
        }

        return string.Empty;
    }


    private void AddNewFolderInFolder(string guid, string newFolderUrl, Folder folder)
    {
        Folder fd = new Folder()
        {
            Guid = guid,
            Url = newFolderUrl
        };
        folder.SubFolders.Add(fd);
    }

    private Folder FindParentFolder(string url, Folder startPoint, bool fromReq)
    {
        bool found = false; 
        if (fromReq)
        {
            found = startPoint.FolderItems.Any(e => e.Url == url);
        }
        else
        {
            found = startPoint.SubFolders.Any(e => e.Url == url);
        }

        if (found)
        {
            return startPoint;
        }
        else
        {
            foreach (Folder newSp in startPoint.SubFolders)
            {
                Folder foundItem = FindParentFolder(url, newSp, fromReq);
                if (foundItem != null)
                {
                    return foundItem;
                }
            }
        }

        return null;
    }

    private void DeleteFolderFromCache(string url)
    {
        bool localDeletionSuccessful = false;

        Folder folder = FindParentFolder(url, _allRequirementsFolders.ElementAt(0), false);
        if (folder != null)
        {
            var itemToRemove = folder.SubFolders.FirstOrDefault(fi => fi.Url == url);
            if (itemToRemove != null)
            {
                localDeletionSuccessful = folder.SubFolders.Remove(itemToRemove);
            }
        }

        //Force the cache loading if could remove the items "programmatically"
        if (!localDeletionSuccessful)
        {
            GetAllRequirements();
        }
    }
}