﻿using Manager.Core.Base;
using Manager.Core.Enumns;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Manager.Core.Business
{
  public class CertificationItem : BaseEntity
  {
    public EnumItemCertification ItemCertification { get; set; }
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string IdItem { get; set; }
    public string Name { get; set; }
    public string Concept { get; set; }
  }
}
