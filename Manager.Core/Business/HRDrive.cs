using Manager.Core.Base;
using Manager.Core.BusinessModel;
using Manager.Views.BusinessCrud;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Linq;
using System.Collections.Generic;
using Manager.Views.BusinessList;

namespace Manager.Core.Business
{
  public class HRDrive : BaseEntity
  {
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idPerson { get; set; }
    public string NamePerson { get; set; }
    public List<AttachmentDrive> Attachments { get; set; }
    public ViewListHRDrive GetListHRDrive()
    {
      return new ViewListHRDrive()
      {
        _id = _id,
        _idPerson = _idPerson,
        NamePerson = NamePerson,
        Attachments = Attachments?.Select(p => p.GetViewCrud()).ToList()
      };
    }
  }
}
