using Manager.Core.BaseV2;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Manager.Core.BusinessV2
{
  public class UserV2 : BasePublic
  {
    public string Name { get; set; }
    [BsonDateTimeOptions(DateOnly = true)]
    public DateTime? DateBirth { get; set; }
    public string Document { get; set; }
    public string PhotoUrl { get; set; }
    public EnumSex Sex { get; set; }
    public string CellPhone { get; set; }
    public ViewListSchooling Schooling { get; set; }

    #region Constructors
    public UserV2() { }
    public UserV2(ViewCrudUserV2 view)
    {
      CellPhone = view.CellPhone;
      DateBirth = view.DateBirth;
      Document = view.Document;
      Name = view.Name;
      PhotoUrl = view.PhotoUrl;
      Sex = view.Sex;
      Schooling = view.Schooling;
      _id = view._id;
      _change = view._change;
    }
    #endregion

    #region Crud
    public ViewCrudUserV2 ToCrud()
    {
      return new ViewCrudUserV2()
      {
        CellPhone = CellPhone,
        DateBirth = DateBirth,
        Document = Document,
        Name = Name,
        PhotoUrl = PhotoUrl,
        Sex = Sex,
        Schooling = Schooling,
        _id = _id,
        _change = _change
      };
    }
    #endregion

    #region List
    public ViewListUserV2 ToList()
    {
      return new ViewListUserV2()
      {
        _id = _id,
        Name = Name,
        Document = Document,
        CellPhone = CellPhone
      };
    }
    #endregion 

  }
}
