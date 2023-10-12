using BlazBeaver.Helpers;
using BlazBeaver.Interfaces;

namespace BlazBeaver.Data;


public class Requirement: IReqProt
{ 
    /*private string _url = string.Empty;
  
    public string Url 
    { 
        get
        {
            return _url;
        }
        set
        {
            _url = value;
            string[] allParts = _url.Split('/');
            string idAndTitle = allParts[allParts.Length - 1];
            
            string idOnly = TitleAndIdTransformation.GetId(idAndTitle);
            string titleOnly = TitleAndIdTransformation.GetTitle(idAndTitle);

            Id = AddSharp(idOnly);
            Title = titleOnly;
        } 
    }*/
    
    public string Url { get; set; } = string.Empty;
    public string Guid { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    //public string Status { get; set; } = string.Empty;
    public List<string> Components { get; set; } = new List<string>();
    public List<string> SoftwareUnits { get; set; } = new List<string>();
    public string SRAIds { get; set; } = string.Empty;
    public string CreatedInVersion { get; set; } = string.Empty;
    public string DeprecatedInVersion { get; set; } = string.Empty;
    public bool IsSafety { get; set; } = false;
    public string Description { get; set; } = string.Empty;
    public string Changelog { get; set; } = string.Empty;
    public string AssociatedProtocolsIds { get; set; } = string.Empty;

    /*private string AddSharp(string rawReq)
    {
        return rawReq.Insert(ReqAndProcProperties.ReqPrefix.Length, "#");
    }*/

}


