using Manager.Core.Base;

namespace Manager.Core.Business
{
  public class TextDefault : BaseEntity
  {
    public string Name { get; set; }
    public string Content { get; set; }
    public Company Company { get; set; }
    public TextDefault Template { get; set; }
  }
}
