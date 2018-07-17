using Manager.Core.Base;

namespace Manager.Core.Business
{
  public class Attachment : BaseEntity
  {
    public string LocalName { get; set; }
    public string Extension { get; set; }
    public long Lenght { get; set; }
    public bool Saved { get; set; }
  }
}
