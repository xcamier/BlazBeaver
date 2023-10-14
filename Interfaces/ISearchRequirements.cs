using BlazBeaver.Data;

public interface ISearchRequirements
{
    IEnumerable<Requirement> Search(SearchInstructions searchInstructions);
}
