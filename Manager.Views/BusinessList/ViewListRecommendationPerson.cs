using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessList
{
  public class ViewListRecommendationPerson: _ViewList
  {
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idRecommendation { get; set; }
    public string NameRecommendation { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idPerson { get; set; }
    public string NamePerson { get; set; }
    public string Image { get; set; }
  }
}
