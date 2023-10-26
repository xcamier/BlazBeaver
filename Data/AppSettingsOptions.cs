namespace BlazBeaver.Data;

public class AppSettingsOptions
{
    public const string AppSettings = "AppSettings";

    public bool DisplayBrowser { get; set; }
    public bool UseApiForUniqueId { get; set; }
    public string APIUrl { get; set; }
    public string RequirementsFolderLocation { get; set; }
    public string ProtocolsFolderLocation { get; set; }
    public string ResourcesLocation { get; set; }
    public string ComponentsSetting { get; set; }        
    public string SoftwareUnitsSetting { get; set; }     
    public string FileExtension { get; set; }             
    public string RequirementIdentifier { get; set; }
    public string RootFolderName { get; set; }
}
