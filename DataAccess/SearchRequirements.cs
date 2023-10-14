using BlazBeaver.Data;
using BlazBeaver.Interfaces;

namespace BlazBeaver.DataAccess;

public class SearchRequirements: ISearchRequirements
{
   IRequirementsRepository _rr;

   public SearchRequirements(IRequirementsRepository rr)
   {
      _rr = rr;
   }

   public IEnumerable<Requirement> Search(SearchInstructions searchInstructions)
   {
      IEnumerable<Folder> folders = _rr.GetAllCachedRequirements();

      List<Requirement> allReqs = new();
      FlattenRequirements(folders, allReqs);

      if (searchInstructions.IsAnd)
      {
         return SearchAnd(searchInstructions.AllCriteria, allReqs);
      }  
      else
      {
         return SearchOr(searchInstructions.AllCriteria, allReqs);
      }
   }

   private static IEnumerable<Requirement> SearchAnd(IEnumerable<SearchCriterion> searchCriteria, IEnumerable<Requirement> allReqs)
   {
      IEnumerable<Requirement> filteredResults = allReqs;
      foreach (SearchCriterion criterion in searchCriteria)
      {
         filteredResults = SelectSubsetOfRequirements(filteredResults, criterion);
      }

      return filteredResults;
   }

    private static IEnumerable<Requirement> SearchOr(IEnumerable<SearchCriterion> searchCriteria, IEnumerable<Requirement> allReqs)
   {
      List<Requirement> filteredResults = new(); 
      foreach (SearchCriterion criterion in searchCriteria)
      {
         IEnumerable<Requirement> partialResults = SelectSubsetOfRequirements(allReqs, criterion);
         
         filteredResults.AddRange(partialResults);
      }

      return filteredResults.Distinct();
   }

   private static void FlattenRequirements(IEnumerable<Folder> folders, List<Requirement> allReqs)
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

    private static IEnumerable<Requirement> SelectSubsetOfRequirements(IEnumerable<Requirement> filteredResults, SearchCriterion criterion)
    {
        if (criterion.TypeOfCriterion == "Components")
        {
            filteredResults = filteredResults.Where(r => r.Components.Contains(criterion.Criterion));
        }
        else if (criterion.TypeOfCriterion == "Software Units")
        {
            filteredResults = filteredResults.Where(r => r.SoftwareUnits.Contains(criterion.Criterion));
        }
        else if (criterion.TypeOfCriterion == "Protocols")
        {
            filteredResults = filteredResults.Where(r => r.AssociatedProtocolsIds.Contains(criterion.Criterion));
        }
        else if (criterion.TypeOfCriterion == "Number")
        {
            filteredResults = filteredResults.Where(r => string.Compare(criterion.Criterion, r.Id, StringComparison.InvariantCultureIgnoreCase) == 0);
        }
        else if (criterion.TypeOfCriterion == "Title")
        {
            filteredResults = filteredResults.Where(r => r.Title.Contains(criterion.Criterion, StringComparison.InvariantCultureIgnoreCase));
        }

        return filteredResults;
    }
}
