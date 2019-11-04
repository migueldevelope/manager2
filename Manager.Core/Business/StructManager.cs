using Manager.Core.Base;
using Manager.Views.BusinessList;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  public class StructManager : BaseEntity
  {

    [BsonRepresentation(BsonType.ObjectId)]
    public string _idManager { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idPerson { get; set; }
    public List<ViewListStructManager> Team { get; set; }
  }
}
