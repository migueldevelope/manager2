using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudHRDrive: _ViewCrud
  {
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idPerson { get; set; }
    public string NamePerson { get; set; }
    public ViewCrudAttachmentDrive Attachments { get; set; }
  }
}
