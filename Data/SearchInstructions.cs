namespace BlazBeaver.Data;

public class SearchInstructions
{
    public bool IsAnd { get; set; } //if not isand -> is a or

    public IEnumerable<SearchCriterion> AllCriteria { get; set; }
}