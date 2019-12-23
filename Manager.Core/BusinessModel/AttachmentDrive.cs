using Manager.Views.BusinessCrud;
using System;

namespace Manager.Core.BusinessModel
{
  public class AttachmentDrive : AttachmentField
  {
    public DateTime? Date { get; set; }
    public ViewCrudAttachmentDrive GetViewCrud()
    {
      return new ViewCrudAttachmentDrive()
      {
        Date = Date,
        Name = Name,
        Url = Url,
        _idAttachment = _idAttachment
      };
    }
  }
}
