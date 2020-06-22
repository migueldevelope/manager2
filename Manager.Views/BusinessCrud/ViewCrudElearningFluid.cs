using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudElearningFluid:_ViewCrud
  {
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idUser { get; set; }
    public ViewListPersonBase Person { get; set; }
    public DateTime? DateBegin { get; set; }
    public DateTime? DateEnd { get; set; }
    public decimal Score { get; set; }
    public List<ViewCrudElearningFluidAnswer> Questions { get; set; }
    public EnumStatusElearningFluid StatusElearningFluid { get; set; }
  }
}
