namespace BlazBeaver.Helpers;

public class TitleAndIdTransformation
{
    public static string GetId(string idAndTitle)
    {
        string[] idAndTitleSplit = idAndTitle.Split(' ');
        string idOnly = idAndTitleSplit[0];

        return idOnly;
    }

    public static string GetTitle(string idAndTitle)
    {
        string[] idAndTitleSplit = idAndTitle.Split(' ');
        string[] titleSplit = new string[idAndTitleSplit.Length - 1];
        Array.Copy(idAndTitleSplit, 1, titleSplit, 0, idAndTitleSplit.Length - 1);
        string titleOnly = string.Join(" ", titleSplit);
        
        if (titleOnly.EndsWith(FilesSettings.FileExtension))
        {
            titleOnly = titleOnly.Substring(0, titleOnly.Length - FilesSettings.FileExtension.Length);  //removes the .md
        }

        return titleOnly;
    }
    
}
