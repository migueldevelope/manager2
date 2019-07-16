﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Manager.Views.BusinessList
{
  public class ViewListPersonBaseManager: ViewListPersonBase
  {
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idManager { get; set; }
    public string NameManager { get; set; }
  }
}