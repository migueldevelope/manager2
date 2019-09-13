using Manager.Views.Enumns;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessList
{
  public class ViewListPersonInfo : _ViewListBase
  {
    public EnumTypeJourney TypeJourney { get; set; }
    public string Occupation { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idManager { get; set; }
    public string Manager { get; set; }
    public string Registration { get; set; }
    public string Schooling { get; set; }
    public DateTime? DateAdm { get; set; }
  }
}
