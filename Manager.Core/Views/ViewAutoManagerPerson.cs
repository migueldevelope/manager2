using Manager.Core.Enumns;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Manager.Core.Views
{
  public class ViewAutoManagerPerson
  {
    [BsonRepresentation(BsonType.ObjectId)]
    public string IdPerson { get; set; }
    public string NamePerson { get; set; }
    public EnumStatusAutoManagerView Status { get; set; }
  }
}
