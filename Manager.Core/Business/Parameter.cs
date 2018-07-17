using Manager.Core.Base;

namespace Manager.Core.Business
{
  public class Parameter : BaseEntity
  {
    public string Name { get; set; }
    public string Content { get; set; }
  }
}
