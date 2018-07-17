using Manager.Core.Base;
using MongoDB.Bson;

namespace Manager.Core.Business
{
  public class Area : BaseEntity
  {
    public string Name { get; set; }
    public Company Company { get; set; }
    public Area Template { get; set; }
  }
}
