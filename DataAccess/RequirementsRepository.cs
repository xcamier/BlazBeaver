using BlazBeaver.Data;
using BlazBeaver.Helpers;
using BlazBeaver.Interfaces;
using Microsoft.Extensions.Options;

namespace BlazBeaver.DataAccess;

public class RequirementsRepository : IRequirementsRepository
{
    public event EventHandler OnStartLoadingRequirements;
    public event EventHandler OnEndLoadingRequirements;

    private IEnumerable<Folder> _allRequirementsFolders;

    private readonly IDataIO _dataIO;
    private readonly DataSourceConverter<Requirement> _converter; 
    private readonly IOptions<AppSettingsOptions> _configuration;

    public RequirementsRepository(IDataIO dataIO, DataSourceConverter<Requirement> converter, IOptions<AppSettingsOptions> configuration)
    {
        _dataIO = dataIO;
        _converter = converter;
        _configuration = configuration;
    }

#region Requirements management
    public IEnumerable<Folder> GetAllRequirements()
    {
        try
        {
            RaiseEvent(OnStartLoadingRequirements);

            IEnumerable<string> rawRequirements = _dataIO.LoadAll(_configuration.Value.RequirementsFolderLocation, _configuration.Value.FileExtension);
            IEnumerable<Folder> requirements = _converter.Convert(rawRequirements, _dataIO);

            _allRequirementsFolders = requirements;

            return requirements;
        }
        finally
        {
            RaiseEvent(OnEndLoadingRequirements);
        }
    }

    public IEnumerable<Folder> GetAllCachedRequirements()
    {
        return _allRequirementsFolders;
    }

    public Requirement GetRequirement(string url)
    {
        string requirementAsString = _dataIO.LoadItem(url);

        if (string.IsNullOrEmpty(requirementAsString))
        {
            return new Requirement();
        }
        else
        {
            Requirement requirement = _converter.Convert(url, requirementAsString);
            
            return requirement;
        }
    }

    public string SaveRequirement(Requirement req, string folderUrl)
    {
        //Saves the previous URL
        string oldUrl = req.Url;

        //Buids the url and adds it to the file
        string reqFileName = string.Concat(Helpers.FileHelpers.CurrationOf(req.Id), _configuration.Value.FileExtension);
        string confirmedFolderUrl = folderUrl;
        if (string.IsNullOrEmpty(folderUrl) || !Directory.Exists(folderUrl))
        {
            confirmedFolderUrl = _configuration.Value.RequirementsFolderLocation;
        }
        string requirementUrl = Path.Combine(confirmedFolderUrl, reqFileName);
        req.Url = requirementUrl;

        string requirementAsText = _converter.Convert(req);

        bool requirementSaveSuccessfully = _dataIO.SaveItem(oldUrl, requirementUrl, requirementAsText);

        if (requirementSaveSuccessfully)
        {
            UpdateRequirementInCache(req);
        }
        else 
        {
            return string.Empty;
        }

        return requirementUrl;
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

    private void RaiseEvent(EventHandler eventToRaise)
    {
        // Make a temporary copy of the event to avoid possibility of
        // a race condition if the last subscriber unsubscribes
        // immediately after the null check and before the event is raised.
        EventHandler raiseEvent = eventToRaise;

        // Event will be null if there are no subscribers
        if (raiseEvent != null)
        {
            // Call to raise the event.
            raiseEvent(this, new EventArgs());
        }
    }

#endregion Requirements management

#region  Folders management

    public bool DeleteFolder(string url)
    {
        bool deletionSuccessful = _dataIO.DeleteItem(url);

        if (deletionSuccessful)
        {
            DeleteFolderFromCache(url);
        }

        return deletionSuccessful;
    }

    public string CreateFolder(string newFolderName, string parentUrl)
    {
        //if root folder, there is no parent
        string fixedUrl = parentUrl == _configuration.Value.RootFolderName ? string.Empty : parentUrl;

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

    private void UpdateRequirementInCache(Requirement req)
    {
        string requirementFoldeUrl = Path.GetDirectoryName(req.Url);
        Folder foundFolder = FolderHelpers.FindFolderFromUrl(requirementFoldeUrl, _allRequirementsFolders.ElementAt(0));

        //if none is found, pick the first one
        if (foundFolder == null)
        {
            foundFolder = _allRequirementsFolders.ElementAt(0);
        }

        IReqProt foundItem = foundFolder.FolderItems.FirstOrDefault(fi => fi.Url == req.Url);
        if (foundItem != null)
        {
            foundFolder.FolderItems.Remove(foundItem);
        };
        foundFolder.FolderItems.Add(req);
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
#endregion Folders management

}
