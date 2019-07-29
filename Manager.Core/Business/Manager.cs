﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Manager.Core.Business
{
  public class Manager
  {

    [BsonRepresentation(BsonType.ObjectId)]
    public string _idManager { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idPerson { get; set; }
  }
}
