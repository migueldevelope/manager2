using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;

namespace Manager.Core.Business
{
  public class BaseHelp: BaseEntity
  {
    public string Name { get; set; }
    public string Content { get; set; }
    public string AccessLink { get; set; }
    public ViewCrudAttachmentField Attachment { get; set; }
    public bool Infra { get; set; }
    public bool Manager { get; set; }
    public bool Employee { get; set; }
    public long AccessCount { get; set; }
    public ViewListBaseHelp GetViewList()
    {
      return new ViewListBaseHelp()
      {
        _id = _id,
        Name = Name,
        Content = Content,
        AccessCount = AccessCount
      };
    }
    public ViewCrudBaseHelp GetViewCrud()
    {
      return new ViewCrudBaseHelp()
      {
        _id = _id,
        Name = Name,
        Manager = Manager,
        Employee = Employee,
        Infra = Infra,
        Content = Content,
        Attachment = Attachment,
        AccessCount = AccessCount,
        AccessLink = AccessLink
      };
    }
  }
}
