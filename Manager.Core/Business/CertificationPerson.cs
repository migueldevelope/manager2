using Manager.Core.Base;
using Manager.Core.Enumns;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Manager.Core.Business
{
  public class CertificationPerson: BaseEntity
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string IdPerson { get; set; }
    public string Name { get; set; }
    public string Mail { get; set; }
    public string TextDefault { get; set; }
    public string TextDefaultEnd { get; set; }
    public EnumStatusCertificationPerson StatusCertificationPerson { get; set; }
    public string Comments { get; set; }

  }
}
