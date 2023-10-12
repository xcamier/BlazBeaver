using System.Text;
using BlazBeaver.Data;
using BlazBeaver.Interfaces;

namespace BlazBeaver.DataAccess;

public class DataSourceConverter<T> where T: IReqProt, new()
{

# region Requirement convertion

    public string Convert(Requirement req)
    {
        StringBuilder sb = new StringBuilder();

        IEnumerable<string> allPropertiesNames = GetPOCOProperties();

        foreach (string propertyName in allPropertiesNames) 
        {
            //Creation of the section
            string sectionName = Helpers.FilesSettings.SectionTpl.Replace("replace_me", propertyName);
            sb.AppendLine(sectionName);
            
            //Creation of the content of the section
            var propertyValue = req.GetType().GetProperty(propertyName).GetValue(req, null);
            string propertyValueAsString = string.Empty;
            string typeOfProperty = req.GetType().GetProperty(propertyName).PropertyType.Name;
            if (typeOfProperty.Contains("List"))
            {
                List<string> propertyValueAsList = propertyValue as List<string>;
                propertyValueAsString = string.Join(",", propertyValueAsList);
            }
            else
            {
                propertyValueAsString = $"{propertyValue}";  //implicit convertion to string  
            }

            sb.AppendLine(propertyValueAsString);
            sb.AppendLine("");
        }        
        
        return sb.ToString();
    }

    private IEnumerable<string> GetPOCOProperties()
    {
        return typeof(Requirement).GetProperties().Select(p => p.Name);
    }

#endregion Requirement convertion


#region Folders convertion
    public IEnumerable<Folder> Convert(IEnumerable<string> folderContent)
    {
        List<Folder> folders = new List<Folder>();

        Folder currentFolder = AddAsNewFolder(string.Empty, Helpers.ReqAndProcProperties.RootFolderName, folders);
        foreach(string item in folderContent)
        {   
            string ext = Path.GetExtension(item);
            if (ext.CompareTo(Helpers.FilesSettings.FileExtension) == 0)
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
#endregion Folders convertion

}

