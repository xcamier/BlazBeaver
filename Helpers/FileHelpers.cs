namespace BlazBeaver.Helpers;

public class FileHelpers
{
    //Removes special characters from the file name 
    public static string CurrationOf(string source)
    {
        char[] forbiden = Path.GetInvalidFileNameChars();
        string curatedString = CleanupString(source, forbiden);

        return curatedString;
    }

    private static string CleanupString(string source, char[] oldChar)
    {
        return String.Join("", source.ToCharArray().Where(a => !oldChar.Contains(a)).ToArray());
    }
}
