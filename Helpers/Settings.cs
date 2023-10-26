namespace BlazBeaver.Helpers;

/*public static class AppSettings
{
    public const bool DisplayBrowser = false;
    public const string APIUrl = "http://localhost:5028/api/v1/Ids/next/";
    public const bool UseApiForUniqueId = false;
}*/

public static class FilesSettings
{
    //public const string RequirementsFolderLocation = @"D:\Git\Perso\tmpremisolreqs\Docs\Requirements";
    //public const string ProtocolsFolderLocation = @"D:\Git\Perso\Remisol\Docs\Protocols";
    //public const string ResourcesLocation = @".\Resources";
    //public const string ComponentsSetting = "Components.txt";
    //public const string SoftwareUnitsSetting = "SoftwareUnits.txt";
    //public const string FileExtension = ".md";
    public const string SectionTpl = "--- replace_me ---";
}

public static class ReqAndProcProperties
{
    //public const string RequirementIdentifier = "RREQ-";
    //public const string RootFolderName = "root";
    public static readonly IEnumerable<string> PropertiesToNotSerialize = new List<string>()
    {
        "Url"
    };

    public static readonly IEnumerable<string> SearchTypes = new List<string>()
    {
        "Components",
        "Software Units",
        "Protocols",
        "Number",
        "Title",
        "Text"
    };

    public static readonly IEnumerable<string> TypesOfRequirement = new List<string>()
    {
        "Display",
        "Functional",
        "Performance",
        "Printing",
        "Report",
        "Technical",
        "Testing",
        "Validate"
    };
}