using Manager.Core.Base;
using Manager.Core.Enumns;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Manager.Core.Business
{
  /// <summary>
  /// Coleção para o item da acreditação
  /// </summary>
  public class CertificationItem : BaseEntity
  {
    public EnumItemCertification ItemCertification { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string IdItem { get; set; }
    public string Name { get; set; }
    public string Concept { get; set; }
  }
}
