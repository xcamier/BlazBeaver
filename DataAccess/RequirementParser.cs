/************************************/
/* Parsing of a requirements file   */
/*                                  */
/* thanks to Charles PAUX           */
/************************************/

using System.Text.RegularExpressions;

namespace BlazBeaver.DataAccess;

public class RequirementParser
{
    Regex regex = new Regex(@"-{3} (?<title>[\s\w]+) -{3}");

    public Dictionary<string, string> Properties {get; private set; } = new Dictionary<string, string>();
    
    public bool Parse(string input)
    {
        if (string.IsNullOrEmpty(input)) 
        { 
            return false;
        }
        
        MatchCollection matchCollection = regex.Matches(input);

        for (int i = 0; i < matchCollection.Count; i++)
        {
            var match = matchCollection[i];
        
            if (i == matchCollection.Count - 1)
            {
                // last title in the file
                ExtractPropertyValue(input.Substring(match.Index));
            }
            else
            {
                var nextMatch = matchCollection[i + 1];
                ExtractPropertyValue(input.Substring(match.Index, nextMatch.Index - match.Index));
            }
        }

        return true;
    }
    
    private void ExtractPropertyValue(string input)
    {
        Match match = regex.Match(input);

        string val = input.Substring(match.Index + match.Length);
        Properties.Add(match.Groups["title"].Value, val.Trim());
    }
}
