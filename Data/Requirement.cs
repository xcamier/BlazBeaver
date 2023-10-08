using BlazBeaver.Helpers;
using BlazBeaver.Interfaces;

namespace BlazBeaver.Data;


public class Requirement: IReqProt
{ 
    private string _url = string.Empty;
  
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
    }
    
    public string Guid { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public bool IsSafety { get; set; } = false;
    public string Description { get; set; } = string.Empty;
    public string Changelog { get; set; } = string.Empty;
    public List<string> AssociatedProtocolsIds { get; set; } = new List<string>();

    private string AddSharp(string rawReq)
    {
        return rawReq.Insert(ReqAndProcProperties.ReqPrefix.Length, "#");
    }

}


