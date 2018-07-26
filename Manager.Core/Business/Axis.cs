using Manager.Core.Base;
using Manager.Core.Enumns;

namespace Manager.Core.Business
{
  public class Axis : BaseEntity
  {
    public string Name { get; set; }
    public EnumTypeAxis TypeAxis { get; set; }
    public Sphere Sphere { get; set; }
    public Axis Template { get; set; }
  }
}
