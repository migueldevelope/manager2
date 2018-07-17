using Manager.Core.Base;
using Manager.Core.Enumns;
using MongoDB.Bson;

namespace Manager.Core.Business
{
  public class Sphere : BaseEntity
  {
    public string Name { get; set; }
    public Company Company { get; set; }
    public Sphere Template { get; set; }
    public EnumTypeSphere TypeSphere { get; set; }
  }
}
