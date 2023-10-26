namespace BlazBeaver.Helpers;

public static class FilesSettings
{
    public static readonly IEnumerable<string> PropertiesToNotSerialize = new List<string>()
    {
        "Url"
    };

    public const string SectionTpl = "--- replace_me ---";
}

public static class GlobalCollections
{
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