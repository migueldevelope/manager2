using Manager.Views.BusinessList;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudSalaryScaleLog : _ViewCrudBase
  {
    public ViewListCompany Company { get; set; }
    public List<ViewListGrade> Grades { get; set; }
    public DateTime? Date { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idSalaryScalePrevious { get; set; }
  }
}
