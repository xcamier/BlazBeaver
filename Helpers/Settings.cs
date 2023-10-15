namespace BlazBeaver.Helpers;

public static class AppSettings
{
    public const bool DisplayBrowser = false;
}

public static class FilesSettings
{
    public const string RequirementsFolderLocation = "/Users/xavier/Developer/testFolder/Requirements";
    public const string ProtocolsFolderLocation = "/Users/xavier/Developer/testFolder/Protocols";
    public const string ResourcesLocation = "./Resources";
    public const string ComponentsSetting = "Components.txt";
    public const string SoftwareUnitsSetting = "SoftwareUnits.txt";
    public const string FileExtension = ".md";
    public const string SectionTpl = "--- replace_me ---";
}

public static class ReqAndProcProperties
{
    public const string RequirementIdentifier = "RREQ-";
    public const string RootFolderName = "root";
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
        "Title"
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