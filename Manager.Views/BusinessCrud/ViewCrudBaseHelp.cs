using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudBaseHelp: _ViewCrudBase
  {
    public string Content { get; set; }
    public bool Infra { get; set; }
    public bool Manager { get; set; }
    public bool Employee { get; set; }
    public string AccessLink { get; set; }
    public ViewCrudAttachmentField Attachment { get; set; }
    public long AccessCount { get; set; }
  }
}
