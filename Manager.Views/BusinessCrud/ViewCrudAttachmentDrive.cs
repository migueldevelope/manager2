using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudAttachmentDrive
  {
    public string Url { get; set; }
    public string Name { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idAttachment { get; set; }
    public DateTime? Date { get; set; }
  }
}
