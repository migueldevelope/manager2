using Manager.Views.BusinessCrud;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Manager.Views.BusinessList
{
  public class ViewListHRDrive: _ViewList
  {
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idPerson { get; set; }
    public string NamePerson { get; set; }
    public List<ViewCrudAttachmentDrive> Attachments { get; set; }
  }
}
