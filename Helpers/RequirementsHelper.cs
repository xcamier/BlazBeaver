using BlazBeaver.Data;
using BlazBeaver.Interfaces;

namespace BlazBeaver.Helpers;


public class RequirementsHelper
{

    //Heper method to suggest an id when creating a new item. As it is not 
    //centralized, the solution is not wonderful and should be replaced by a 
    //dedicated service
    public static string GetNewId(IEnumerable<Folder> requirementsInFolders)
    {
        List<Requirement> allReqs = new();
        FlattenRequirements(requirementsInFolders, allReqs);

        int maxId = -1;
        foreach (Requirement req in allReqs)
        {
            string idAsStr =  req.Id.Substring(ReqAndProcProperties.RequirementIdentifier.Length);
            int id;
            bool success = int.TryParse(idAsStr, out id);
            if (success)
            {
                maxId = id > maxId ? id: maxId;
            }
        }
        //We want the next bigger id
        maxId++;

        return FormatRequirement(maxId);
    }

    public static string FormatRequirement(int uniqueId)
    {
        return $"{ReqAndProcProperties.RequirementIdentifier}{uniqueId}";
    }

    public static void FlattenRequirements(IEnumerable<Folder> folders, List<Requirement> allReqs)
    {
        foreach (Folder folder in folders)
        {
            foreach(IReqProt toto in folder.FolderItems)
            {
                allReqs.Add(toto as Requirement);
            }

            FlattenRequirements(folder.SubFolders, allReqs);
        }
    }

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