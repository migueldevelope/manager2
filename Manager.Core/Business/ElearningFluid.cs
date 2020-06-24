using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  public class ElearningFluid : BaseEntity
  {
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idUser { get; set; }
    public ViewListPersonBase Person { get; set; }
    public DateTime? DateBegin { get; set; }
    public DateTime? DateEnd { get; set; }
    public decimal Score { get; set; }
    public bool ElearningVideo { get; set; }
    public bool ElearningCertificate { get; set; }
    public List<ViewCrudElearningFluidAnswer> Questions { get; set; }
    public EnumStatusElearningFluid StatusElearningFluid { get; set; }

    public ViewCrudElearningFluid GetViewCrud()
    {
      return new ViewCrudElearningFluid()
      {
        _id = _id,
        Person = Person,
        DateBegin = DateBegin,
        DateEnd = DateEnd,
        Score = Score,
        Questions = Questions,
        _idUser = _idUser,
        StatusElearningFluid = StatusElearningFluid
      };
    }

    public ViewListElearningFluid GetViewList()
    {
      return new ViewListElearningFluid()
      {
        _id = _id,
      };
    }
  }
}
