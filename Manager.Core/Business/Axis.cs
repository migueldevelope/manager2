using Manager.Core.Base;
using Manager.Core.Enumns;
using MongoDB.Bson;

namespace Manager.Core.Business
{
  public class Axis : BaseEntity
  {
    public string Name { get; set; }
    public Axis Template { get; set; }
    public Company Company { get; set; }
    public Sphere Sphere { get; set; }
    public EnumTypeAxis TypeAxis { get; set; }
  }
}
