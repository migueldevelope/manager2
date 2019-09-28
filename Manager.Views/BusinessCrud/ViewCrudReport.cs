using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudReport
  {
    public string _id { get; set; }
    public string Name { get; set; }
    public string _idAccount { get; set; }
    public EnumStatusReport StatusReport { get; set; }
    public string Link { get; set; }
  }
}
