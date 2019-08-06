using Manager.Views.BusinessList;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudSalaryScale : _ViewCrudBase 
  {
    public ViewListCompany Company { get; set; }
    public DateTime? Date { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idSalaryScalePrevious { get; set; }
  }
}
