using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Manager.Core.Business
{
  public class FeelingDay: BaseEntity
  {
    public EnumFeeling Feeling { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idUser { get; set; }
    public DateTime? Date { get; set; }

    public ViewListFeelingDay GetViewList()
    {
      return new ViewListFeelingDay()
      {
        Date = Date,
        _id = _id,
        _idUser = _idUser
      };
    }

    public ViewCrudFeelingDay GetViewCrud()
    {
      return new ViewCrudFeelingDay()
      {
        Date = Date,
        _id = _id,
        Feeling = Feeling,
        _idUser = _idUser
      };
    }
  }
}
