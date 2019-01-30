using Manager.Core.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Core.Business.Integration
{
  public class IntegrationBase : BaseEntity
  {
    public string Key { get; set; }
    public string Name { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idCompany { get; set; }
  }
}
