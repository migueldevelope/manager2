using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessList
{
  public class ViewListOccupationResume: _ViewListBase
  {
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idGroup { get; set; }
    public string NameGroup { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idArea { get; set; }
    public ViewListCbo Cbo { get; set; }
    public string Description { get; set; }
  }
}
