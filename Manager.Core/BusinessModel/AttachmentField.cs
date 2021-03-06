﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Manager.Core.BusinessModel
{
  /// <summary>
  /// Coleção para lista de anexos utilizado em todas os locais de lista de anexos
  /// </summary>
  public class AttachmentField
  {
    public string Url { get; set; }
    public string Name { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idAttachment { get; set; }
  }
}
