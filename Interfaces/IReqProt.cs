namespace BlazBeaver.Interfaces;

public interface IReqProt : IFolderElement
{
    string Guid { get; set; }
    string Url { get; set; }
    string Id { get; set; }
    string Title { get; set; }
}
