using System.Reflection;
using System.Text;
using BlazBeaver.Data;
using BlazBeaver.Interfaces;
using Microsoft.Extensions.Options;

namespace BlazBeaver.DataAccess;

public class DataSourceConverter<T> where T: IReqProt, new()
{
    private readonly IOptions<AppSettingsOptions> _configuration;

    public DataSourceConverter(IOptions<AppSettingsOptions> configuration)
    {
        _configuration = configuration;
    }

    public IEnumerable<Folder> Convert(IEnumerable<string> folderContent, IDataIO dataIO)
    {
        List<Folder> folders = new List<Folder>();

        Folder currentFolder = AddAsNewFolder(string.Empty, _configuration.Value.RootFolderName, folders);
        foreach(string itemUrl in folderContent)
        {   
            if (Helpers.FileHelpers.IsPathAFile(itemUrl))
            {
                string itemAsString = dataIO.LoadItem(itemUrl);
                if (!string.IsNullOrEmpty(itemAsString))
                {
                    T newItem = ConvertToT(itemUrl, itemAsString);

                    currentFolder.FolderItems.Add(newItem);
                }
            }
            else
            {
                //It's a folder
                currentFolder = AddAsNewFolder(string.Empty, itemUrl, folders);
            }
        }

        return folders;
    }

    #region Requirement convertion

    public string Convert(Requirement req)
    {
        StringBuilder sb = new StringBuilder();

        IEnumerable<string> allPropertiesNames = GetPOCOProperties();

        foreach (string propertyName in allPropertiesNames.
                                            Where(pn => !Helpers.ReqAndProcProperties.PropertiesToNotSerialize.Contains(pn))) 
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

    public T Convert(string itemUrl, string itemContent)
    {
        if (string.IsNullOrEmpty(itemUrl))
        {
            return new T();
        }
        else
        {
            return ConvertToT(itemUrl, itemContent);
        }
    }

    private IEnumerable<string> GetPOCOProperties()
    {
        return typeof(T).GetProperties().Select(p => p.Name);
    }

   private static T ConvertToT(string itemUrl, string itemAsString)
    {
        //It's a req 
        T newItem = new T()
        {
            Url = itemUrl
        };

        RequirementParser ps = new RequirementParser();
        bool parsingOk = ps.Parse(itemAsString);
        if (parsingOk)
        {
            Type type = typeof(T);
            foreach (var p in ps.Properties)
            {
                PropertyInfo property = type.GetProperty(p.Key);

                if (property.PropertyType == typeof(string))
                {
                    property.SetValue(newItem, p.Value, null);
                }
                else if (property.PropertyType == typeof(bool))
                {
                    property.SetValue(newItem, System.Convert.ToBoolean(p.Value, null));
                }
                else if (property.PropertyType == typeof(List<string>))
                {
                    string[] extractedValues = p.Value.Split(",");
                    List<string> extractedValuesAsList = extractedValues.ToList();
                    extractedValuesAsList.RemoveAll(v => string.IsNullOrEmpty(v));

                    property.SetValue(newItem, extractedValuesAsList, null);
                }
                else
                {
                    Console.WriteLine("DataSourceConverter > Convert: property type not managed");
                }
            }
        }

        return newItem;
    }

#endregion Requirement convertion


#region Folders convertion    
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
        if (currentFolder.Url == _configuration.Value.RootFolderName)
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

