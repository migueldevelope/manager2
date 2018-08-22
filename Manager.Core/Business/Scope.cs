using Manager.Core.Base;

namespace Manager.Core.Business
{
  public class Scope : BaseEntity
  {
    public string Name { get; set; }
    public long Order { get; set; }
  }
}
