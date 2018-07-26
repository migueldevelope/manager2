using Manager.Core.Base;
using Manager.Core.Enumns;
using MongoDB.Bson;

namespace Manager.Core.Business
{
    public class Schooling : BaseEntity
  {
    public string Name { get; set; }
    public EnumTypeSchooling Type { get; set; }
    public Schooling Template { get; set; }
  }
}
