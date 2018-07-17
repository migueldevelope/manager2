using Manager.Core.Base;
using MongoDB.Bson;

namespace Manager.Core.Business
{
    public class Schooling : BaseEntity
  {
    public string Name { get; set; }
    public Schooling Template { get; set; }
  }
}
