using Manager.Core.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Manager.Core.Business
{
  public class NewsletterViews:BaseEntity
  {
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idPerson { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idNewsletter { get; set; }
  }
}
