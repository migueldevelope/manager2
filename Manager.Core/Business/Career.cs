using Manager.Core.Base;
using Manager.Core.Enumns;
using MongoDB.Bson;

namespace Manager.Core.Business
{
  public class Career : BaseEntity
  {
    public string Name { get; set; }
    public EnumTypeCareer Type { get; set; }
    public Career Template { get; set; }
  }
}
